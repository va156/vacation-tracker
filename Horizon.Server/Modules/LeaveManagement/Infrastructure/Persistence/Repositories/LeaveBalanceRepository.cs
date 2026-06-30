using Horizon.Server.Modules.LeaveManagement.Domain.Entities;
using Horizon.Server.Modules.LeaveManagement.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Server.Modules.LeaveManagement.Infrastructure.Persistence.Repositories;

public class LeaveBalanceRepository : ILeaveBalanceRepository
{
    private readonly AppDbContext _context;

    public LeaveBalanceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<LeaveBalance>> GetByEmployeeIdAsync(int employeeId, int? year = null)
    {
        var query = _context.Set<LeaveBalance>()
            .Include(b => b.LeaveType)
            .Where(b => b.EmployeeId == employeeId && b.IsActive);

        if (year.HasValue)
            query = query.Where(b => b.Year == year.Value);

        return await query.OrderBy(b => b.LeaveType.Name).ToListAsync();
    }

    public async Task<IReadOnlyList<LeaveBalance>> GetAllForYearAsync(int year)
    {
        return await _context.Set<LeaveBalance>()
            .Include(b => b.LeaveType)
            .Include(b => b.Employee)
            .Where(b => b.Year == year && b.IsActive)
            .ToListAsync();
    }

    public async Task<LeaveBalance?> GetByIdAsync(int id)
    {
        return await _context.Set<LeaveBalance>()
            .Include(b => b.LeaveType)
            .FirstOrDefaultAsync(b => b.Id == id && b.IsActive);
    }

    public async Task AddAsync(LeaveBalance balance)
    {
        await _context.Set<LeaveBalance>().AddAsync(balance);
    }

    public async Task UpdateAsync(LeaveBalance balance)
    {
        _context.Set<LeaveBalance>().Update(balance);
        await Task.CompletedTask;
    }
}
