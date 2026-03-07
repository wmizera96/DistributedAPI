using DistributedAPI.Books.Application;
using DistributedAPI.Books.Application.Model;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(nameof(BooksPolicy.Read))]
    public async Task<IActionResult> GetBooks(CancellationToken cancellationToken)
    {
        return Ok(await _booksService.GetBooksAsync(cancellationToken));
    }
    
    [HttpPost]
    [Authorize(nameof(BooksPolicy.Write))]
    public async Task<IActionResult> PostBook(Book book, CancellationToken cancellationToken)
    {
        return Ok(book);
    }
}