using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Horizon.Server.API.Filters;

/// <summary>
/// Global action filter that runs FluentValidation validators against every
/// action argument before the controller method executes.
/// When a registered <see cref="IValidator{T}"/> exists for an argument type and
/// validation fails, the filter short-circuits the pipeline and returns
/// <c>422 Unprocessable Entity</c> with a <see cref="ValidationProblemDetails"/> body.
/// This complements the built-in DataAnnotations model-state validation.
/// </summary>
public class ValidationFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initialises the filter with the application's DI service provider.
    /// </summary>
    /// <param name="serviceProvider">Used to resolve <c>IValidator&lt;T&gt;</c> at runtime.</param>
    public ValidationFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Validates each action argument that has a registered FluentValidation validator.
    /// Execution continues only when all arguments pass validation.
    /// </summary>
    /// <param name="context">The action execution context containing bound action arguments.</param>
    /// <param name="next">Delegate to invoke the action if validation succeeds.</param>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null) continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            var validator = _serviceProvider.GetService(validatorType) as IValidator;

            if (validator is null) continue;

            var validationContext = new ValidationContext<object>(argument);
            var result = await validator.ValidateAsync(validationContext);

            if (!result.IsValid)
            {
                var errors = result.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                context.Result = new UnprocessableEntityObjectResult(new ValidationProblemDetails(errors)
                {
                    Title = "Validation failed",
                    Status = StatusCodes.Status422UnprocessableEntity
                });
                return;
            }
        }

        await next();
    }
}
