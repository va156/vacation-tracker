using Horizon.Server.Modules.Users.Application.DTOs;
using Horizon.Server.Modules.Users.Domain.Interfaces;
using Horizon.Server.Modules.Users.Infrastructure.Persistence.Repositories;
using Horizon.Server.Modules.Users.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.Server.API.Controllers;

/// <summary>
/// Handles all authentication-related HTTP endpoints: registration, login,
/// token refresh, logout, and retrieving the current user's profile.
/// The refresh token is transported via an HTTP-only <c>Secure</c> cookie named
/// <c>refreshToken</c> to prevent access from client-side JavaScript.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<AuthController> _logger;

    /// <summary>Initialises the controller with required authentication services.</summary>
    public AuthController(
        IAuthenticationService authenticationService,
        IUserRepository userRepository,
        ILogger<AuthController> logger)
    {
        _authenticationService = authenticationService;
        _userRepository = userRepository;
        _logger = logger;
    }

    /// <summary>
    /// Registers a new user account and returns an access token with user profile.
    /// </summary>
    /// <param name="request">Registration details validated by <c>RegisterRequestDtoValidator</c>.</param>
    /// <returns><c>200 OK</c> with <see cref="AuthResponseDto"/>; <c>400 Bad Request</c> if the email or username is taken.</returns>
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
                Title = "Registration error",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    /// <summary>
    /// Authenticates a user with email and password.
    /// </summary>
    /// <param name="request">Credentials validated by <c>LoginRequestDtoValidator</c>.</param>
    /// <returns><c>200 OK</c> with <see cref="AuthResponseDto"/>; <c>401 Unauthorized</c> on invalid credentials or deactivated account.</returns>
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
                Title = "Login error",
                Detail = ex.Message,
                Status = StatusCodes.Status401Unauthorized
            });
        }
    }

    /// <summary>
    /// Rotates the refresh token stored in the HTTP-only cookie and issues a new token pair.
    /// </summary>
    /// <returns><c>200 OK</c> with a new <see cref="AuthResponseDto"/>; <c>401 Unauthorized</c> if the cookie is missing or the token is invalid.</returns>
    [HttpPost("refresh-token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken()
    {
        try
        {
            var refreshToken = Request.Cookies["refreshToken"] ??
                throw new UnauthorizedAccessException("Refresh token not found");

            var ipAddress = GetIpAddress();
            var result = await _authenticationService.RefreshTokenAsync(refreshToken, ipAddress);

            SetRefreshTokenCookie(result.RefreshToken);

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new ProblemDetails
            {
                Title = "Token refresh error",
                Detail = ex.Message,
                Status = StatusCodes.Status401Unauthorized
            });
        }
    }

    /// <summary>
    /// Revokes the current refresh token and clears the HTTP-only cookie.
    /// Always returns <c>200 OK</c> to avoid leaking token validity information.
    /// </summary>
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

            return Ok(new { message = "Logout successful" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            return Ok(new { message = "Logout successful" });
        }
    }

    /// <summary>
    /// Returns the profile of the currently authenticated user based on the <c>userId</c> claim in the JWT.
    /// </summary>
    /// <returns><c>200 OK</c> with <see cref="UserDto"/>; <c>401</c> if the token is invalid; <c>404</c> if the user no longer exists.</returns>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                _logger.LogWarning("Attempt to get user info without a valid userId claim in the token");
                return Unauthorized(new ProblemDetails
                {
                    Title = "Unauthorized",
                    Detail = "Invalid token",
                    Status = StatusCodes.Status401Unauthorized
                });
            }

            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} was not found in the database", userId);
                return NotFound(new ProblemDetails
                {
                    Title = "User not found",
                    Detail = $"User with ID {userId} does not exist",
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

            _logger.LogInformation("User profile {UserId} retrieved successfully", userId);
            return Ok(userDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving current user profile");
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Internal server error",
                Detail = "An error occurred while processing the request",
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }

    #region Private Methods

    /// <summary>
    /// Extracts the client IP address from the <c>X-Forwarded-For</c> header (reverse proxy scenario)
    /// or falls back to the direct connection remote address.
    /// </summary>
    private string GetIpAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"]!;

        return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
    }

    /// <summary>
    /// Writes the refresh token to an HTTP-only, Secure, SameSite=Strict cookie
    /// with a 7-day expiry so that it is not accessible from JavaScript.
    /// </summary>
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
