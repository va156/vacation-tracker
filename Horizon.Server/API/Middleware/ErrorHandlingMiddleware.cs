using System.Text.Json;

namespace Horizon.Server.API.Middleware;

/// <summary>
/// Global error-handling middleware that catches unhandled exceptions propagating
/// through the ASP.NET Core pipeline and converts them into structured
/// <c>application/json</c> problem-detail responses.
/// This prevents raw stack traces from reaching clients in production
/// and ensures a uniform error response shape across all endpoints.
/// Full implementation to be added when the global exception handling feature is developed.
/// </summary>
public class ErrorHandlingMiddleware
{
}
