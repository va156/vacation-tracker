using Horizon.Server.Modules.LeaveManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.Server.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LeaveBalancesController : ControllerBase
{
    private readonly ILeaveBalanceRepository _leaveBalanceRepository;
    private readonly ILogger<LeaveBalancesController> _logger;

    public LeaveBalancesController(ILeaveBalanceRepository leaveBalanceRepository, ILogger<LeaveBalancesController> logger)
    {
        _leaveBalanceRepository = leaveBalanceRepository;
        _logger = logger;
    }

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
