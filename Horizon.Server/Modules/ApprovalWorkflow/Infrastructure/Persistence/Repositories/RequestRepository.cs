using Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;
using Horizon.Server.Modules.ApprovalWorkflow.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Server.Modules.ApprovalWorkflow.Infrastructure.Persistence.Repositories;

public class RequestRepository : IRequestRepository
{
    private readonly AppDbContext _context;

    public RequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Request>> GetAllByEmployeeIdAsync(int employeeId)
    {
        return await _context.Set<Request>()
            .Include(r => r.Status)
            .Where(r => r.EmployeeId == employeeId && r.IsActive)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Request>> GetAllAsync()
    {
        return await _context.Set<Request>()
            .Include(r => r.Status)
            .Where(r => r.IsActive)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<Request?> GetByIdAsync(int id)
    {
        return await _context.Set<Request>()
            .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);
    }

    public async Task<Request?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Set<Request>()
            .Include(r => r.Status)
            .Include(r => r.ApprovalTemplate)
            .Include(r => r.Leaves)
            .Include(r => r.ApprovalHistory)
            .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);
    }

    public async Task AddAsync(Request request)
    {
        await _context.Set<Request>().AddAsync(request);
    }

    public async Task UpdateAsync(Request request)
    {
        _context.Set<Request>().Update(request);
        await Task.CompletedTask;
    }
}
