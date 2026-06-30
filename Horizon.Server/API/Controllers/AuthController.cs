using Horizon.Server.Modules.Users.Application.DTOs;
using Horizon.Server.Modules.Users.Domain.Interfaces;
using Horizon.Server.Modules.Users.Infrastructure.Persistence.Repositories;
using Horizon.Server.Modules.Users.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.Server.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthenticationService authenticationService,
        IUserRepository userRepository,
        ILogger<AuthController> logger)
    {
        _authenticationService = authenticationService;
        _userRepository = userRepository;
        _logger = logger;
    }

    [HttpPost("register")]
    [AllowAnonymous] 
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterRequestDto request)
    {
        try
        {
            var ipAddress = GetIpAddress();
            var result = await _authenticationService.RegisterAsync(request, ipAddress);

            SetRefreshTokenCookie(result.RefreshToken);

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Ошибка регистрации",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginRequestDto request)
    {
        try
        {
            var ipAddress = GetIpAddress();
            var result = await _authenticationService.LoginAsync(request, ipAddress);

            SetRefreshTokenCookie(result.RefreshToken);

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new ProblemDetails
            {
                Title = "Ошибка входа",
                Detail = ex.Message,
                Status = StatusCodes.Status401Unauthorized
            });
        }
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken()
    {
        try
        {
            var refreshToken = Request.Cookies["refreshToken"] ??
                throw new UnauthorizedAccessException("Refresh token не найден");

            var ipAddress = GetIpAddress();
            var result = await _authenticationService.RefreshTokenAsync(refreshToken, ipAddress);

            SetRefreshTokenCookie(result.RefreshToken);

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new ProblemDetails
            {
                Title = "Ошибка обновления токена",
                Detail = ex.Message,
                Status = StatusCodes.Status401Unauthorized
            });
        }
    }

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                var ipAddress = GetIpAddress();
                await _authenticationService.RevokeTokenAsync(refreshToken, ipAddress);
            }

            Response.Cookies.Delete("refreshToken");

            return Ok(new { message = "Выход выполнен успешно" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при выходе из системы");
            return Ok(new { message = "Выход выполнен успешно" });
        }
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        try
        {
            // Получаем ID пользователя из claims JWT токена
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                _logger.LogWarning("Попытка получить информацию о пользователе без валидного userId в токене");
                return Unauthorized(new ProblemDetails
                {
                    Title = "Неавторизован",
                    Detail = "Недействительный токен",
                    Status = StatusCodes.Status401Unauthorized
                });
            }

            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                _logger.LogWarning("Пользователь с ID {UserId} не найден в базе", userId);
                return NotFound(new ProblemDetails
                {
                    Title = "Пользователь не найден",
                    Detail = $"Пользователь с ID {userId} не существует",
                    Status = StatusCodes.Status404NotFound
                });
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                Phone = user.Phone,
                RoleId = user.RoleId,
                RoleName = user.Role?.Name ?? "Unknown",
                EmployeeId = user.EmployeeId,
                IsEmployee = user.IsEmployee,
                LastLoginAt = user.LastLoginAt
            };

            _logger.LogInformation("Информация о пользователе {UserId} успешно получена", userId);
            return Ok(userDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении информации о текущем пользователе");
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Внутренняя ошибка сервера",
                Detail = "Произошла ошибка при обработке запроса",
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }

    #region Private Methods

    private string GetIpAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            return Request.Headers["X-Forwarded-For"]!;
        }

        return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
    }

    private void SetRefreshTokenCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }

    #endregion
}