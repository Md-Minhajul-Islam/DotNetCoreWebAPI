using LibraryManagementAPI.Data;
using LibraryManagementAPI.Entities;
using LibraryManagementAPI.Repositories;

namespace LibraryManagementAPI.UnitOfWorks;

public class UnitOfWork
{
    private readonly AppDbContext _context;

    private AuditLogRepository? _auditLog;
    private BookRepository? _book;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public AuditLogRepository AuditLog => _auditLog ??= new AuditLogRepository(_context);
    public BookRepository Book => _book ??= new BookRepository(_context);
    
}