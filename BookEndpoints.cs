namespace Bookstore;

public static class BookEndpoints
{
    public static void MapBookEndpoints(this WebApplication app)
    {
        // Sample database
        List<Book> booksDb = new()
        {
            new Book { Id = 1, Title = "Clean Code", Author = "Robert C. Martin", Price = 29.99 },
            new Book { Id = 2, Title = "The Pragmatic Programmer", Author = "Andrew Hunt", Price = 24.99 },
            new Book { Id = 3, Title = "C# in Depth", Author = "Jon Skeet", Price = 34.50 },
            new Book { Id = 4, Title = "Refactoring", Author = "Martin Fowler", Price = 19.99 }
        };

        // Search books by author
        app.MapGet("/api/books/search", (string author) =>
            booksDb.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)));

        // List books under a max price
        app.MapGet("/api/books/bargains", (double maxPrice) =>
            booksDb.Where(b => b.Price < maxPrice));
    }
}
