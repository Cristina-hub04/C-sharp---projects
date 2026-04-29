using TheInventorySystem;
using TheInventorySystem.Models;

class Program
{

    static void Main(string[] args)
    {
        using var db = new StoreContext();

        if (!db.Categories.Any())
        {
            var clothing= new Category { Name = "Clothing" };
            var electronics = new Category { Name = "Electronics" };

            db.Categories.AddRange(electronics, clothing);
            db.Products.Add(new Product { Name = "Laptop", Price = 1000, Category = electronics });
            db.Products.Add(new Product { Name = "T-Shirt", Price = 35, Category = clothing });

            db.SaveChanges();
        }

        Console.WriteLine("Database ready");

        Console.WriteLine(db.Products.Count());
    }
}
