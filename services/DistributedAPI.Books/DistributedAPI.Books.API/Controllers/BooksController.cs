using DistributedAPI.Books.Application;
using Microsoft.AspNetCore.Mvc;

namespace DistributedAPI.Books.API.Controllers;

public class BooksController : BaseController
{
    private readonly IBooksService _booksService;

    public BooksController(IBooksService booksService)
    {
        _booksService = booksService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetBooks(CancellationToken cancellationToken)
    {
        return Ok(await _booksService.GetBooksAsync(cancellationToken));
    }
}