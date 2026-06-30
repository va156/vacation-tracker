using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.Server.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] 
public class EmployeesController : ControllerBase
{
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(ILogger<EmployeesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Policy = "HRAndAbove")]
    public IActionResult GetAllEmployees()
    {
        return Ok(new { message = "Список сотрудников (только для HR и Admin)" });
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "EmployeeOnly")]
    public IActionResult GetEmployeeById(int id)
    {
        var currentUserId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

        // логика проверки доступа

        return Ok(new { employeeId = id, message = "Информация о сотруднике" });
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult CreateEmployee()
    {
        return Ok(new { message = "Сотрудник создан (только Admin)" });
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult DeleteEmployee(int id)
    {
        return Ok(new { message = "Сотрудник удален (только Admin)" });
    }
}