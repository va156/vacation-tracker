using FluentValidation;
using Horizon.Server.Modules.Users.Application.DTOs;

namespace Horizon.Server.Modules.Users.Application.Validators;

public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Имя пользователя обязательно")
            .MinimumLength(3).WithMessage("Минимум 3 символа")
            .MaximumLength(50).WithMessage("Максимум 50 символов")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Только буквы, цифры и _");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email обязателен")
            .EmailAddress().WithMessage("Неверный формат email")
            .MaximumLength(200).WithMessage("Максимум 200 символов");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обязателен")
            .MinimumLength(8).WithMessage("Минимум 8 символов")
            .Matches(@"[A-Z]").WithMessage("Пароль должен содержать заглавную букву")
            .Matches(@"[0-9]").WithMessage("Пароль должен содержать цифру");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Имя обязательно")
            .MaximumLength(100).WithMessage("Максимум 100 символов");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Фамилия обязательна")
            .MaximumLength(100).WithMessage("Максимум 100 символов");

        RuleFor(x => x.MiddleName)
            .MaximumLength(100).WithMessage("Максимум 100 символов")
            .When(x => x.MiddleName != null);

        RuleFor(x => x.Phone)
            .Matches(@"^\+?\d{10,15}$").WithMessage("Неверный формат телефона")
            .When(x => !string.IsNullOrEmpty(x.Phone));
    }
}
