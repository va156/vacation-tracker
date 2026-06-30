using Horizon.Server.Modules.Users.Application.DTOs;
using Horizon.Server.Modules.Users.Domain.Entities;
using Horizon.Server.Modules.Users.Domain.Interfaces;
using Horizon.Server.Modules.Users.Public;
using Horizon.Server.Modules.Shared.Domain.Abstractions;

namespace Horizon.Server.Modules.Users.Infrastructure.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtSettings _jwtSettings;

    public AuthenticationService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IUnitOfWork unitOfWork,
        JwtSettings jwtSettings)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
        _jwtSettings = jwtSettings;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, string ipAddress)
    {
        // Проверяем, не занят ли email
        var existingUserByEmail = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUserByEmail != null)
        {
            throw new InvalidOperationException("Пользователь с таким email уже существует");
        }

        // Проверяем, не занят ли username
        var existingUserByUsername = await _userRepository.GetByUsernameAsync(request.Username);
        if (existingUserByUsername != null)
        {
            throw new InvalidOperationException("Пользователь с таким именем уже существует");
        }

        var defaultRole = await _userRepository.GetRoleByCodeAsync("Employee");
        if (defaultRole == null)
        {
            throw new InvalidOperationException("Роль по умолчанию не найдена в системе. Убедитесь что миграция SeedRoles была применена.");
        }

        // Хешируем пароль
        var passwordHash = _passwordHasher.HashPassword(request.Password);

        // Создаем пользователя
        var user = new User(
            username: request.Username,
            email: request.Email,
            passwordHash: passwordHash,
            firstName: request.FirstName,
            lastName: request.LastName,
            roleId: defaultRole.Id,
            createdBy: 0 // Система 
        );

        // Сохраняем пользователя
        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // Генерируем токены
        var accessToken = _jwtTokenGenerator.GenerateAccessToken(user);
        var refreshToken = await GenerateAndSaveRefreshToken(user.Id, ipAddress);

        // Возвращаем ответ
        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            User = MapToUserDto(user)
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request, string ipAddress)
    {
        // Ищем пользователя по email
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Неверный email или пароль");
        }

        // Проверяем пароль
        var isValidPassword = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
        if (!isValidPassword)
        {
            throw new UnauthorizedAccessException("Неверный email или пароль");
        }

        // Обновляем время последнего входа
        user.UpdateLastLogin();
        await _unitOfWork.SaveChangesAsync();

        // Генерируем токены
        var accessToken = _jwtTokenGenerator.GenerateAccessToken(user);
        var refreshToken = await GenerateAndSaveRefreshToken(user.Id, ipAddress);

        // Возвращаем ответ
        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            User = MapToUserDto(user)
        };
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, string ipAddress)
    {
        // Ищем refresh token в БД
        var token = await _userRepository.GetRefreshTokenAsync(refreshToken);

        if (token == null || !token.IsActive)
        {
            throw new UnauthorizedAccessException("Недействительный refresh token");
        }

        // Получаем пользователя
        var user = await _userRepository.GetByIdAsync(token.UserId);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Пользователь не найден");
        }

        // Отзываем старый токен
        token.Revoke(ipAddress);

        // Генерируем новый refresh token
        var newRefreshToken = await GenerateAndSaveRefreshToken(user.Id, ipAddress);

        // Сохраняем изменения
        await _unitOfWork.SaveChangesAsync();

        // Генерируем новый access token
        var accessToken = _jwtTokenGenerator.GenerateAccessToken(user);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken.Token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            User = MapToUserDto(user)
        };
    }

    public async Task RevokeTokenAsync(string refreshToken, string ipAddress)
    {
        var token = await _userRepository.GetRefreshTokenAsync(refreshToken);

        if (token == null || !token.IsActive)
        {
            throw new UnauthorizedAccessException("Недействительный refresh token");
        }

        token.Revoke(ipAddress);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task<RefreshToken> GenerateAndSaveRefreshToken(int userId, string ipAddress)
    {
        var tokenString = _jwtTokenGenerator.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);

        var refreshToken = new RefreshToken(tokenString, userId, expiresAt, ipAddress);

        await _userRepository.AddRefreshTokenAsync(refreshToken);
        await _unitOfWork.SaveChangesAsync();

        return refreshToken;
    }

    private UserDto MapToUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            MiddleName = user.MiddleName,
            RoleId = user.RoleId,
            RoleName = user.Role?.Name,
            IsEmployee = user.IsEmployee
        };
    }
}