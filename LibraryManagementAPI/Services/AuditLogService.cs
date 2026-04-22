
using LibraryManagementAPI.Entities;
using LibraryManagementAPI.UnitOfWorks;

namespace LibraryManagementAPI.Services;

public class AuditLogService
{
    private readonly UnitOfWork _unitOfWork;

    public AuditLogService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddAuditLog(string entityName, string action)
    {
        var auditLog = new AuditLog
        {
            EntityName = entityName,
            Action = action,
            OccurredAt = DateTime.UtcNow
        };

        await _unitOfWork.AuditLog.AddAsync(auditLog);
    }

}