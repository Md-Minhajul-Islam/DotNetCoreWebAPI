using LibraryManagementAPI.Entities;
using LibraryManagementAPI.UnitOfWorks;

namespace LibraryManagementAPI.Services;

public class BookService
{
    private readonly UnitOfWork _unitOfWork;

    public BookService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Book> AddBookAsync(Book book)
    {
        return await _unitOfWork.Book.AddAsync(book);
    }

}