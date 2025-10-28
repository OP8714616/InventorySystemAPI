using System;

namespace InventorySystemAPI.Models
{
    public class Product
    {
        // 商品編號(主鍵)
        public int Id { get; set; }

        // 商品名稱
        public string Name { get; set; } = string.Empty;

        // 商品描述
        public string? Description { get; set; }

        // 商品價格
        public decimal Price { get; set; }

        // 庫存數量
        public int Stock { get; set; }

        // 建立時間
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}