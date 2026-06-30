using Horizon.Server.Modules.LeaveManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.Server.API.Controllers;

/// <summary>
/// Exposes leave balance information. Employees may only view their own balances;
/// HR Managers and Admins may query any employee's balances or retrieve all balances
/// for a given year.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LeaveBalancesController : ControllerBase
{
    private readonly ILeaveBalanceRepository _leaveBalanceRepository;
    private readonly ILogger<LeaveBalancesController> _logger;

    /// <summary>Initialises the controller with the leave balance repository and logger.</summary>
    public LeaveBalancesController(ILeaveBalanceRepository leaveBalanceRepository, ILogger<LeaveBalancesController> logger)
    {
        _leaveBalanceRepository = leaveBalanceRepository;
        _logger = logger;
    }

    /// <summary>
    /// Returns the leave balances of the currently authenticated user for the specified year
    /// (defaults to the current calendar year).
    /// </summary>
    /// <param name="year">Optional calendar year; defaults to the current year.</param>
    [HttpGet("my")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyBalances([FromQuery] int? year)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var currentYear = year ?? DateTime.UtcNow.Year;
        var balances = await _leaveBalanceRepository.GetByEmployeeIdAsync(userId, currentYear);

        return Ok(balances.Select(b => new
        {
            b.Id,
            b.EmployeeId,
            LeaveType = b.LeaveType?.Name,
            LeaveTypeCode = b.LeaveType?.Code,
            b.Year,
            b.Entitled,
            b.Used,
            b.Planned,
            Available = b.Available
        }));
    }

    /// <summary>
    /// Returns leave balances for a specific employee. Requires the <c>HRAndAbove</c> policy.
    /// </summary>
    /// <param name="employeeId">Primary key of the target employee.</param>
    /// <param name="year">Optional calendar year; defaults to the current year.</param>
    [HttpGet("employee/{employeeId:int}")]
    [Authorize(Policy = "HRAndAbove")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(int employeeId, [FromQuery] int? year)
    {
        var currentYear = year ?? DateTime.UtcNow.Year;
        var balances = await _leaveBalanceRepository.GetByEmployeeIdAsync(employeeId, currentYear);

        return Ok(balances.Select(b => new
        {
            b.Id,
            b.EmployeeId,
            LeaveType = b.LeaveType?.Name,
            b.Year,
            b.Entitled,
            b.Used,
            b.Planned,
            Available = b.Available
        }));
    }

    /// <summary>
    /// Returns leave balances for all employees for the specified year.
    /// Requires the <c>HRAndAbove</c> policy.
    /// </summary>
    /// <param name="year">Optional calendar year; defaults to the current year.</param>
    [HttpGet("all")]
    [Authorize(Policy = "HRAndAbove")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int? year)
    {
        var currentYear = year ?? DateTime.UtcNow.Year;
        var balances = await _leaveBalanceRepository.GetAllForYearAsync(currentYear);

        return Ok(balances.Select(b => new
        {
            b.Id,
            b.EmployeeId,
            LeaveType = b.LeaveType?.Name,
            b.Year,
            b.Entitled,
            b.Used,
            b.Planned,
            Available = b.Available
        }));
    }
}
