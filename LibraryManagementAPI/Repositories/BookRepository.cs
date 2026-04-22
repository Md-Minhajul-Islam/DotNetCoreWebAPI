using LibraryManagementAPI.Data;
using LibraryManagementAPI.Entities;

namespace LibraryManagementAPI.Repositories;

public class BookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Book> AddAsync(Book book)
    {
        await _context.Books.AddAsync(book);

        await _context.SaveChangesAsync();

        return book;
    }
}