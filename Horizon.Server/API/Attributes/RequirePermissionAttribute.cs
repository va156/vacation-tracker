using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.Server.API.Attributes;

/// <summary>
/// Convenience attribute that applies an authorization policy named
/// <c>Permission:{permission}</c> to a controller or action method.
/// The corresponding policies must be registered in <c>Program.cs</c>.
/// </summary>
/// <example>
/// <code>[RequirePermission("balance:adjust")]</code>
/// is equivalent to
/// <code>[Authorize(Policy = "Permission:balance:adjust")]</code>
/// </example>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
public class RequirePermissionAttribute : AuthorizeAttribute
{
    /// <summary>
    /// Initialises the attribute and resolves the policy name from the given permission string.
    /// </summary>
    /// <param name="permission">
    /// The permission claim value (e.g. <c>"request:view-all"</c>).
    /// The applied policy name will be <c>Permission:{permission}</c>.
    /// </param>
    public RequirePermissionAttribute(string permission)
    {
        Policy = $"Permission:{permission}";
    }
}
