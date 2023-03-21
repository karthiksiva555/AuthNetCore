using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityResourceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private static readonly Book[] Books = {
        new() { Name = "Book 1", Author = "Author 1", Publisher = "Publisher 1"},
        new() { Name = "Book 2", Author = "Author 2", Publisher = "Publisher 2"},
        new() { Name = "Book 3", Author = "Author 3", Publisher = "Publisher 3"}
    };

    private readonly ILogger<BookController> _logger;

    public BookController(ILogger<BookController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    //[Authorize]
    [Authorize(Policy = "read:books", Roles = "reader,creator")]
    public IEnumerable<Book> Get()
    {
        return Books;
    }
}