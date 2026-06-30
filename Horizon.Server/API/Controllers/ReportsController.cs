using Horizon.Server.Modules.ApprovalWorkflow.Domain.Interfaces;
using Horizon.Server.Modules.LeaveManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.Server.API.Controllers;

/// <summary>
/// Provides aggregated reporting endpoints for HR Managers and Admins.
/// All endpoints require the <c>HRAndAbove</c> policy.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "HRAndAbove")]
public class ReportsController : ControllerBase
{
    private readonly IRequestRepository _requestRepository;
    private readonly ILeaveBalanceRepository _leaveBalanceRepository;
    private readonly ILogger<ReportsController> _logger;

    /// <summary>Initialises the controller with the required repositories and logger.</summary>
    public ReportsController(
        IRequestRepository requestRepository,
        ILeaveBalanceRepository leaveBalanceRepository,
        ILogger<ReportsController> logger)
    {
        _requestRepository = requestRepository;
        _leaveBalanceRepository = leaveBalanceRepository;
        _logger = logger;
    }

    /// <summary>
    /// Returns a leave balance summary grouped by leave type for the specified year,
    /// showing total entitled, used, planned and available days across all employees.
    /// </summary>
    /// <param name="year">Optional calendar year; defaults to the current year.</param>
    [HttpGet("leave-summary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLeaveSummary([FromQuery] int? year)
    {
        var currentYear = year ?? DateTime.UtcNow.Year;
        var balances = await _leaveBalanceRepository.GetAllForYearAsync(currentYear);

        var summary = balances
            .GroupBy(b => b.LeaveType?.Name ?? "Unknown")
            .Select(g => new
            {
                LeaveType = g.Key,
                TotalEmployees = g.Count(),
                TotalEntitled = g.Sum(b => b.Entitled),
                TotalUsed = g.Sum(b => b.Used),
                TotalPlanned = g.Sum(b => b.Planned),
                TotalAvailable = g.Sum(b => b.Available)
            });

        return Ok(new { Year = currentYear, Data = summary });
    }

    /// <summary>
    /// Returns a leave request count summary grouped by status,
    /// along with the total number of requests.
    /// </summary>
    [HttpGet("requests-summary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRequestsSummary()
    {
        var requests = await _requestRepository.GetAllAsync();

        var summary = requests
            .GroupBy(r => r.Status?.Name ?? "Unknown")
            .Select(g => new
            {
                Status = g.Key,
                Count = g.Count()
            });

        return Ok(new
        {
            Total = requests.Count,
            ByStatus = summary
        });
    }
}
