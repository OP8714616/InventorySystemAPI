using System;

namespace InventorySystemAPI.Models
{
    public class Inventory
    {
        //庫存記錄編號 (主鍵)
        public int Id { get; set; }
        //商品編號(外鍵)
        public int ProductId { get; set; }
        // 類型：進貨(In) 或 出貨(Out)
        public string Type { get; set; } = string.Empty;
        // 數量(正數表示增加，負數表示減少)
        public int Quantity { get; set; }

        // 備註
        public string? Remarks { get; set; }

        // 操作時間
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // 導航屬性：關聯到商品
        public Product? Product { get; set; }
    }
}