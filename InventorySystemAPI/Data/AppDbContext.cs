using Microsoft.EntityFrameworkCore;
using InventorySystemAPI.Models;

namespace InventorySystemAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // 商品資料表
        public DbSet<Product> Products { get; set; }
        // 庫存記錄資料表
        public DbSet<Inventory> Inventories { get; set; }
    }
}
