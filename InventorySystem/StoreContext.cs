using Microsoft.EntityFrameworkCore;
using TheInventorySystem.Models;

namespace TheInventorySystem;

public class StoreContext : DbContext
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=store.db");
    }
}
