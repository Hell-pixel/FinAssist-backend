using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using FinAssist.Domain;
using FinAssist.Domain.Dtos.Account;
using FinAssist.Domain.Entities;
using FinAssist.Domain.Notification;
using FinAssist.Domain.Repositories;
using FinAssist.Domain.Services.Identity;
using FinAssist.Domain.Services.Notification;
using FinAssist.Infrastructure.Configuration;
using FinAssist.Infrastructure.Exceptions;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace FinAssist.Infrastructure.Services.Identity;

public class AccountService : IAccountService
{
    private const int MaxFailedAccessAttempts = 3;
    private static readonly TimeSpan AccountLockoutSpan = TimeSpan.FromMinutes(15);
    private const string TokenType = "Bearer";

    private readonly IUserRepository _userRepository;
    private readonly IHashService _hashService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly IUserSessionRepository _userSessionRepository;
    private readonly INotificationService _notificationService;

    public AccountService(
        IUserRepository userRepository,
        IHashService hashService,
        ICurrentUserService currentUserService,
        IMapper mapper,
        IUserSessionRepository userSessionRepository,
        INotificationService notificationService)
    {
        _userRepository = userRepository;
        _hashService = hashService;
        _currentUserService = currentUserService;
        _mapper = mapper;
        _userSessionRepository = userSessionRepository;
        _notificationService = notificationService;
    }

    public async Task<AccountTokenDto> Login(LoginUserDto userDto, string userAgent, string? realIp)
    {
        var userEntity = await _userRepository.GetByEmail(userDto.Email);
        if (userEntity is null)
        {
            throw new DataException("Ошибка авторизации. Неверные учетные данные.");
        }

        if (userEntity.LockoutEnabled && userEntity.LockoutEnd > DateTime.UtcNow)
        {
            throw new DataException("Аккаунт временно заблокирован. Попробуйте позже.");
        }

        // if (!userEntity.EmailConfirmed)
        // {
        //     throw new AuthorizationException("Email не подтвержден. Пожалуйста, подтвердите ваш email.");
        // }

        var isValid = _hashService.VerifyPassword(userDto.Password, userEntity.Hash, userEntity.Salt);
        if (!isValid)
        {
            userEntity.AccessFailedCount++;
            if (userEntity.AccessFailedCount >= MaxFailedAccessAttempts)
            {
                userEntity.LockoutEnabled = true;
                userEntity.LockoutEnd = DateTime.UtcNow.Add(AccountLockoutSpan);
            }

            await _userRepository.Update(userEntity);
            throw new DataException("Ошибка авторизации. Неверные учетные данные.");
        }

        userEntity.AccessFailedCount = 0;
        userEntity.LockoutEnabled = false;
        userEntity.LockoutEnd = null;
        await _userRepository.Update(userEntity);

        var refreshToken = _hashService.GenerateRefreshToken();
        var refreshTokenHash = _hashService.HashRefreshToken(refreshToken);

        var session = new UserSessionEntity
        {
            UserId = userEntity.Id,
            ExpiresAt = DateTime.UtcNow.AddSeconds(AppConfiguration.JwtConfiguration.SessionLifeTime),
            UserAgent = userAgent,
            RefreshTokenHash = refreshTokenHash,
            RefreshTokenExpiresAt = DateTime.UtcNow.AddSeconds(AppConfiguration.JwtConfiguration.RefreshTokenLifeTime),
            IpAddress = realIp
        };

        await _userSessionRepository.Insert(session);

        var token = GetAuthToken(session);

        await _notificationService.Send(userEntity.Id, NotificationTemplateType.SuccessLogin,
            new Dictionary<string, string>
            {
                { "UserAgent", userAgent },
                { "IPAddress", realIp ?? "Неизвестно" },
                { "LoginTime", DateTime.UtcNow.ToString("dd.MM.yyyy HH:mm UTCz") },
                { "SessionManagementUrl", $"{AppConfiguration.NotificationConfiguration.AppUrl}/account/sessions" } // TODO: replace with real url
            });

        return new AccountTokenDto
        {
            Token = token,
            TokenType = TokenType,
            ExpiresIn = AppConfiguration.JwtConfiguration.SessionLifeTime,
            RefreshToken = refreshToken,
            RefreshTokenExpiresIn = AppConfiguration.JwtConfiguration.RefreshTokenLifeTime
        };
    }

    public async Task<AccountTokenDto> RefreshSession(string refreshToken)
    {
        var refreshTokenHash = _hashService.HashRefreshToken(refreshToken);

        var session = await _userSessionRepository.GetByRefreshTokenHash(refreshTokenHash);
        if (session is null || session.RefreshTokenExpiresAt < DateTime.UtcNow)
        {
            throw new NotFoundException();
        }

        var newRefreshToken = _hashService.GenerateRefreshToken();
        var newRefreshTokenHash = _hashService.HashRefreshToken(newRefreshToken);

        session.RefreshTokenHash = newRefreshTokenHash;
        session.ExpiresAt = DateTime.UtcNow.AddSeconds(AppConfiguration.JwtConfiguration.SessionLifeTime);
        session.RefreshTokenExpiresAt =
            DateTime.UtcNow.AddSeconds(AppConfiguration.JwtConfiguration.RefreshTokenLifeTime);

        await _userSessionRepository.Update(session);

        var token = GetAuthToken(session);

        return new AccountTokenDto
        {
            Token = token,
            TokenType = TokenType,
            ExpiresIn = AppConfiguration.JwtConfiguration.SessionLifeTime,
            RefreshToken = newRefreshToken,
            RefreshTokenExpiresIn = AppConfiguration.JwtConfiguration.RefreshTokenLifeTime
        };
    }

    public async Task ChangePassword(UserPasswordChangeDto userPasswordChangeDto)
    {
        var userId = _currentUserService.User.Id;

        var user = await _userRepository.GetById(userId);
        if (user is null)
        {
            throw NotFoundException.With<UserEntity>(userId);
        }

        var isVerify = _hashService.VerifyPassword(userPasswordChangeDto.CurrentPassword, user.Hash, user.Salt);
        if (!isVerify)
        {
            throw new ValidationException("Текущий пароль введён не верно");
        }

        var (hash, salt) = _hashService.GenerateHash(userPasswordChangeDto.NewPassword);
        user.Hash = hash;
        user.Salt = salt;

        await _userRepository.Update(user);
    }

    public async Task Register(CreateUserDto createUserDto)
    {
        var existsByEmail = await _userRepository.ExistsByEmail(createUserDto.Email);

        if (existsByEmail)
        {
            throw new ValidationException("Пользователь с таким email уже существует.");
        }

        var (hash, salt) = _hashService.GenerateHash(createUserDto.Password);

        var userEntity = _mapper.Map<CreateUserDto, UserEntity>(createUserDto);
        userEntity.Hash = hash;
        userEntity.Salt = salt;
        userEntity.EmailConfirmed = false;
        userEntity.AccessFailedCount = 0;
        userEntity.LockoutEnabled = false;
        userEntity.CreatedAt = DateTime.UtcNow;

        await _userRepository.Insert(userEntity);

        // todo:: send confirmation email
    }

    public Task<CurrentUserModel> GetCurrentUserInfo()
    {
        return Task.FromResult(_currentUserService.User);
    }

    public async Task<IEnumerable<UserSessionDto>> GetSessions()
    {
        var userId = _currentUserService.User.Id;
        var sessions = await _userSessionRepository.GetByUserId(userId);

        return _mapper.Map<IEnumerable<UserSessionEntity>, IEnumerable<UserSessionDto>>(sessions);
    }

    public async Task Logout()
    {
        var currentSessionId = _currentUserService.User.CurrentSessionId;
        await _userSessionRepository.Delete(currentSessionId);
    }

    public async Task LogoutAllSessions()
    {
        var userId = _currentUserService.User.Id;
        await _userSessionRepository.DeleteAllByUserId(userId);
    }

    private static string GetAuthToken(UserSessionEntity session)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, session.Id.ToString())
        };
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return token;
    }

    private static SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(AppConfiguration.JwtConfiguration.SecurityKey);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private static JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials,
        IEnumerable<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: AppConfiguration.JwtConfiguration.Issuer,
            audience: AppConfiguration.JwtConfiguration.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(AppConfiguration.JwtConfiguration.SessionLifeTime),
            notBefore: DateTime.UtcNow,
            signingCredentials: signingCredentials);

        return tokenOptions;
    }
}