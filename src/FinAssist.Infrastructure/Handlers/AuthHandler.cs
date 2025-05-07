using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using FinAssist.Domain;
using FinAssist.Domain.Entities;
using FinAssist.Domain.Repositories;
using FinAssist.Domain.Services.Identity;
using FinAssist.Infrastructure.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FinAssist.Infrastructure.Handlers;

public class AuthHandler : AuthenticationHandler<AppAuthenticationSchemeOptions>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserSessionRepository _userSessionRepository;

    public AuthHandler(
        IOptionsMonitor<AppAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ICurrentUserService currentUserService, 
        IUserSessionRepository userSessionRepository) : base(options, logger, encoder)
    {
        _currentUserService = currentUserService;
        _userSessionRepository = userSessionRepository;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var token))
        {
            return AuthenticateResult.NoResult();
        }

        if (!ValidateJwtToken(token, out var validatedToken))
        {
            return AuthenticateResult.Fail("Token not valid");
        }

        var userSession = await GetSessionFromToken(validatedToken);

        if (!UserSessionIsAvailable(userSession))
        {
            if(userSession is not null)
            {
                await _userSessionRepository.Delete(userSession.Id);
            }
            
            return AuthenticateResult.Fail("Session not valid");
        }
        
        await UpdateSessionAuditInfo(userSession!);
        SetCurrentUserFromSession(userSession!);
        
        var identity = new GenericIdentity(AppConfiguration.JwtConfiguration.Issuer, "JWT");
        identity.AddClaims(validatedToken.Claims);

        var result = AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name));
        return result;
    }

    private static bool ValidateJwtToken(string token, out JwtSecurityToken securityToken)
    {
        try
        {
            token = token.Split(' ')[1];
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(AppConfiguration.JwtConfiguration.SecurityKey));

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = key,
                ValidIssuer = AppConfiguration.JwtConfiguration.Issuer,
                ValidAudience = AppConfiguration.JwtConfiguration.Audience,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            securityToken = (JwtSecurityToken)validatedToken;

            return true;
        }
        catch
        {
            // ignored
        }

        securityToken = null!;
        return false;
    }

    private void SetCurrentUserFromSession(UserSessionEntity userSession)
    {
        var userEntity = userSession.User!;
        var user = new CurrentUserModel
        {
            Id = userEntity.Id,
            Email = userEntity.Email,
            Name = userEntity.Name,
            CurrentSessionId = userSession.Id
        };

        _currentUserService.Set(user);
    }
    
    private async Task<UserSessionEntity?> GetSessionFromToken(JwtSecurityToken token)
    {
        var sessionId = token.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

        var session = await _userSessionRepository.GetByIdWithUserInfo(Guid.Parse(sessionId));

        return session;
    }
    
    private static bool UserSessionIsAvailable(UserSessionEntity? session)
    {
        return session is { User: not null } && session.ExpiresAt > DateTime.UtcNow;
    }

    private async Task UpdateSessionAuditInfo(UserSessionEntity userSession)
    {
        userSession.IpAddress = Context.Items["RealIp"]?.ToString();
        userSession.UserAgent = Request.Headers.UserAgent.ToString();
        await _userSessionRepository.Update(userSession);
    }
}