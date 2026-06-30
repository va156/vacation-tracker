using Horizon.Server.Modules.ApprovalWorkflow.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.Server.API.Controllers;

/// <summary>
/// Manages leave requests. All endpoints require authentication.
/// Viewing another employee's request requires the <c>request:view-all</c> permission
/// or the <c>Admin</c> role; otherwise only the request owner can retrieve its details.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RequestsController : ControllerBase
{
    private readonly IRequestRepository _requestRepository;
    private readonly ILogger<RequestsController> _logger;

    /// <summary>Initialises the controller with the request repository and logger.</summary>
    public RequestsController(IRequestRepository requestRepository, ILogger<RequestsController> logger)
    {
        _requestRepository = requestRepository;
        _logger = logger;
    }

    /// <summary>
    /// Returns all active leave requests belonging to the currently authenticated user.
    /// </summary>
    /// <returns><c>200 OK</c> with a list of request summaries; <c>401</c> if the token is missing or invalid.</returns>
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

    /// <summary>
    /// Returns all active leave requests across all employees.
    /// Requires the <c>CanViewAllRequests</c> policy (HR Manager or Admin).
    /// </summary>
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

    /// <summary>
    /// Returns the full details of a single leave request, including leaves and approval history.
    /// Access is restricted to the request's owner unless the caller has the
    /// <c>request:view-all</c> permission or is an Admin.
    /// </summary>
    /// <param name="id">Primary key of the request.</param>
    /// <returns><c>200 OK</c>; <c>404</c> if not found; <c>403</c> if access is denied.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var request = await _requestRepository.GetByIdWithDetailsAsync(id);
        if (request == null)
            return NotFound(new ProblemDetails { Title = "Request not found", Status = 404 });

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
