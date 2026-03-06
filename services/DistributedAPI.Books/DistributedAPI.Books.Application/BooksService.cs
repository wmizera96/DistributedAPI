using DistributedAPI.Books.Application.Model;

namespace DistributedAPI.Books.Application;

public interface IBooksService
{
    Task<IEnumerable<Book>> GetBooksAsync(CancellationToken cancellationToken);
}

public class BooksService : IBooksService
{
    public async Task<IEnumerable<Book>> GetBooksAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        
        return new List<Book>
        {
            new Book("Harry Potter", "J. K. Rowling", new DateTime(2021, 12, 10)),
            new Book("Tthe Lord of the Rings", "J. R. R. Tolkiem", new DateTime(1988, 1, 13)),
        };
    }
}