using Horizon.Server.Modules.ApprovalWorkflow.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.Server.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RequestsController : ControllerBase
{
    private readonly IRequestRepository _requestRepository;
    private readonly ILogger<RequestsController> _logger;

    public RequestsController(IRequestRepository requestRepository, ILogger<RequestsController> logger)
    {
        _requestRepository = requestRepository;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyRequests()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var requests = await _requestRepository.GetAllByEmployeeIdAsync(userId);
        return Ok(requests.Select(r => new
        {
            r.Id,
            r.RequestNumber,
            r.EmployeeId,
            Status = r.Status?.Name,
            StatusCode = r.Status?.Code,
            r.CreatedAt,
            r.SubmittedAt,
            r.CompletedAt,
            r.Comment
        }));
    }

    [HttpGet("all")]
    [Authorize(Policy = "CanViewAllRequests")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var requests = await _requestRepository.GetAllAsync();
        return Ok(requests.Select(r => new
        {
            r.Id,
            r.RequestNumber,
            r.EmployeeId,
            Status = r.Status?.Name,
            StatusCode = r.Status?.Code,
            r.CreatedAt,
            r.SubmittedAt,
            r.CompletedAt
        }));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var request = await _requestRepository.GetByIdWithDetailsAsync(id);
        if (request == null)
            return NotFound(new ProblemDetails { Title = "Заявка не найдена", Status = 404 });

        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        int.TryParse(userIdClaim, out var userId);
        var canViewAll = User.HasClaim(c => c.Type == "permission" && c.Value == "request:view-all")
                         || User.IsInRole("Admin");

        if (!canViewAll && request.EmployeeId != userId)
            return Forbid();

        return Ok(new
        {
            request.Id,
            request.RequestNumber,
            request.EmployeeId,
            Status = request.Status?.Name,
            StatusCode = request.Status?.Code,
            request.CreatedAt,
            request.SubmittedAt,
            request.CompletedAt,
            request.Comment,
            request.CurrentStageNumber,
            LeavesCount = request.Leaves.Count,
            ApprovalHistoryCount = request.ApprovalHistory.Count
        });
    }
}
