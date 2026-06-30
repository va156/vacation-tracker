using FluentValidation;
using MediatR;

namespace Horizon.Server.Modules.Shared.Application.Behaviours;

/// <summary>
/// MediatR pipeline behaviour that automatically validates every incoming request
/// before the handler is invoked. If one or more <see cref="IValidator{T}"/> instances
/// are registered for <typeparamref name="TRequest"/>, all are executed and their
/// failures are aggregated. A <see cref="ValidationException"/> is thrown when any
/// validation rule fails, preventing the handler from executing.
/// </summary>
/// <typeparam name="TRequest">The MediatR request type being handled.</typeparam>
/// <typeparam name="TResponse">The response type returned by the handler.</typeparam>
public sealed class ValidationPipelineBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>
    /// Initialises the behaviour with all validators registered for <typeparamref name="TRequest"/>.
    /// </summary>
    /// <param name="validators">Zero or more validators sourced from the DI container.</param>
    public ValidationPipelineBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <summary>
    /// Runs all registered validators; calls the next delegate only when validation succeeds.
    /// </summary>
    /// <param name="request">The incoming request to validate.</param>
    /// <param name="next">Delegate to the next behaviour or the actual handler.</param>
    /// <param name="cancellationToken">Token to observe for cancellation.</param>
    /// <returns>The handler response when validation passes.</returns>
    /// <exception cref="ValidationException">
    /// Thrown with all accumulated failures when at least one rule is violated.
    /// </exception>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count > 0)
            throw new ValidationException(failures);

        return await next();
    }
}
