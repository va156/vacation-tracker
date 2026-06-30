using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.Server.API.Controllers;

/// <summary>
/// Manages employee records. All endpoints require authentication.
/// List and create/delete operations are restricted to HR Managers and Admins;
/// reading a single employee's profile is available to any authenticated user
/// (subject to ownership checks for non-admin callers).
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly ILogger<EmployeesController> _logger;

    /// <summary>Initialises the controller with the logger.</summary>
    public EmployeesController(ILogger<EmployeesController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Returns the list of all employees. Requires the <c>HRAndAbove</c> policy.
    /// Full implementation pending the Employees feature development.
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "HRAndAbove")]
    public IActionResult GetAllEmployees()
    {
        return Ok(new { message = "Employee list (HR and Admin only)" });
    }

    /// <summary>
    /// Returns the profile of a single employee by primary key.
    /// Requires the <c>EmployeeOnly</c> policy; additional ownership checks
    /// should be applied before returning another employee's data.
    /// </summary>
    /// <param name="id">Primary key of the employee.</param>
    [HttpGet("{id}")]
    [Authorize(Policy = "EmployeeOnly")]
    public IActionResult GetEmployeeById(int id)
    {
        var currentUserId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

        return Ok(new { employeeId = id, message = "Employee information" });
    }

    /// <summary>
    /// Creates a new employee record. Requires the <c>AdminOnly</c> policy.
    /// Full implementation pending the Employees feature development.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult CreateEmployee()
    {
        return Ok(new { message = "Employee created (Admin only)" });
    }

    /// <summary>
    /// Soft-deletes an employee record. Requires the <c>AdminOnly</c> policy.
    /// Full implementation pending the Employees feature development.
    /// </summary>
    /// <param name="id">Primary key of the employee to delete.</param>
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult DeleteEmployee(int id)
    {
        return Ok(new { message = "Employee deleted (Admin only)" });
    }
}
