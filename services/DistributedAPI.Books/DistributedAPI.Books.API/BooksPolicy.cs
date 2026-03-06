using DistributedAPI.CommonTools;

namespace DistributedAPI.Books.API;

public class BooksPolicy : BasePolicy
{
    private BooksPolicy(string name, string requiredRole) : base(name, requiredRole)
    {
    }
    
    public static BooksPolicy Write => new BooksPolicy(nameof(Write), "Books.Write");
    public static BooksPolicy Read => new BooksPolicy(nameof(Read), "Books.Read");
}