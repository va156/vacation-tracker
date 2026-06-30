using Horizon.Server.Modules.ApprovalWorkflow.Domain.Interfaces;
using Horizon.Server.Modules.LeaveManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.Server.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "HRAndAbove")]
public class ReportsController : ControllerBase
{
    private readonly IRequestRepository _requestRepository;
    private readonly ILeaveBalanceRepository _leaveBalanceRepository;
    private readonly ILogger<ReportsController> _logger;

    public ReportsController(
        IRequestRepository requestRepository,
        ILeaveBalanceRepository leaveBalanceRepository,
        ILogger<ReportsController> logger)
    {
        _requestRepository = requestRepository;
        _leaveBalanceRepository = leaveBalanceRepository;
        _logger = logger;
    }

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
