using FluentValidation;

namespace Horizon.Server.Modules.ApprovalWorkflow.Application.Commands.CreateRequest;

public class CreateRequestCommandValidator : AbstractValidator<CreateRequestCommand>
{
    public CreateRequestCommandValidator()
    {
        RuleFor(x => x.OperationTypeId)
            .GreaterThan(0).WithMessage("Тип операции обязателен");

        RuleFor(x => x.EmployeeId)
            .GreaterThan(0).WithMessage("Сотрудник обязателен");

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0).WithMessage("Отдел обязателен");

        RuleFor(x => x.ApprovalTemplateId)
            .GreaterThan(0).WithMessage("Шаблон согласования обязателен");
    }
}
