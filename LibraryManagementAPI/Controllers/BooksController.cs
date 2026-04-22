using LibraryManagementAPI.Entities;
using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly BookService _bookService;

    public BooksController(BookService bookService)
    {
        _bookService = bookService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(Book book)
    {
        try
        {
            var createdBook = await _bookService.AddBookAsync(book);
            return Ok(createdBook);
        }
        catch(Exception ex)
        {
            return Conflict(new {message = ex.Message});
        }
    }
}