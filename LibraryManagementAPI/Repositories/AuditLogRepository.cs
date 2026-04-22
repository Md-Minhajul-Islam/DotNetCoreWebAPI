using LibraryManagementAPI.Data;
using LibraryManagementAPI.Entities;

namespace LibraryManagementAPI.Repositories;

public class AuditLogRepository
{
    private readonly AppDbContext _context;
    public AuditLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AuditLog auditLog)
    {
        await _context.AuditLogs.AddAsync(auditLog);
        // await _context.SaveChangesAsync();
    }
}