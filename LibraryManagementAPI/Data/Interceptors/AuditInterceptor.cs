using LibraryManagementAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LibraryManagementAPI.Data.Interceptors;

public class AuditInterceptor : SaveChangesInterceptor
{

   
   // private readonly AuditLogService _auditLogService;
   // public AuditInterceptor(AuditLogService auditLogService)
   // {
   //     _auditLogService = auditLogService;
   // }
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if(eventData.Context is null)
        {
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        
        var context = eventData.Context;

        // var logs = new List<(string entityName, string action)>();
        var logs = new List<AuditLog>();
        
        foreach(var entry in context.ChangeTracker.Entries())
        {

            if(entry.Entity is AuditLog) continue;

            string entityName = entry.Entity.GetType().Name;
            string? action = entry.State switch
            {
                EntityState.Added => "Added",
                EntityState.Modified => "Modified",
                EntityState.Deleted => "Deleted",
                _ => null
            };

            if(!string.IsNullOrWhiteSpace(entityName)
                && !string.IsNullOrWhiteSpace(action))
            {
                // logs.Add((entityName, action));
                logs.Add(new AuditLog
                {
                    EntityName = entityName,
                    Action = action,
                    OccurredAt = DateTime.UtcNow
                });
            }
        }

        // foreach(var log in logs)
        // {
        //     await _auditLogService.AddAuditLog(log.EntityName, log.Action);
        // }
        if (logs.Count > 0)
        {
            context.Set<AuditLog>().AddRange(logs);
        }
        
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}