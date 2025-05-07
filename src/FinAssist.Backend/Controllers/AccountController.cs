using FinAssist.Domain;
using FinAssist.Domain.Dtos.Account;
using FinAssist.Domain.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinAssist.Backend.Controllers;

[ApiController]
[Route("api/v1/account")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    /// <param name="loginUserDto">Модель авторизации пользователя</param>
    /// <returns>Результат авторизации</returns>
    [HttpPost]
    [Route("login")]
    public async Task<AccountTokenDto> Login([FromBody] LoginUserDto loginUserDto)
    {
        var userAgent = Request.Headers.UserAgent.ToString();
        var realIp = HttpContext.Items["RealIp"]?.ToString();
        
        return await _accountService.Login(loginUserDto, userAgent, realIp);
    }
    
    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    /// <param name="createUserDto">Модель регистрации пользователя</param>
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
    {
        await _accountService.Register(createUserDto);
        return NoContent();
    }
    
    /// <summary>
    /// Смена пароля пользователя
    /// </summary>
    /// <param name="changePasswordDto">Модель смены пароля пользователя</param>
    [Authorize]
    [HttpPost]
    [Route("password/change")]
    public async Task<IActionResult> ChangePassword([FromBody] UserPasswordChangeDto changePasswordDto)
    {
        await _accountService.ChangePassword(changePasswordDto);
        return NoContent();
    }
    
    /// <summary>
    /// Получение информации о текущем пользователе
    /// </summary>
    /// <returns>Информация о текущем пользователе</returns>
    [Authorize]
    [HttpGet]
    [Route("current")]
    public async Task<CurrentUserModel> GetCurrentUserInfo()
    {
        return await _accountService.GetCurrentUserInfo();
    }
    
    /// <summary>
    /// Получение списка активных сессий
    /// </summary>
    /// <returns>Список активных сессий</returns>
    [Authorize]
    [HttpGet]
    [Route("sessions")]
    public async Task<IEnumerable<UserSessionDto>> GetSessions()
    {
        return await _accountService.GetSessions();
    }
    
    /// <summary>
    /// Обновление сессии
    /// </summary>
    [HttpPut]
    [Route("sessions/refresh")]
    [AllowAnonymous]
    public async Task<AccountTokenDto> RefreshSession([FromBody] RefreshSessionDto dto)
    {
        return await _accountService.RefreshSession(dto.RefreshToken);
    }
    
    /// <summary>
    /// Завершение текущей сессии
    /// </summary>
    [Authorize]
    [HttpGet]
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        await _accountService.Logout();
        return NoContent();
    }
    
    /// <summary>
    /// Завершение всех активных сессий
    /// </summary>
    [Authorize]
    [HttpPost]
    [Route("logout/all")]
    public async Task<IActionResult> LogoutAllSessions()
    {
        await _accountService.LogoutAllSessions();
        return NoContent();
    }
}
