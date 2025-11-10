using System;
using System.Collections.Generic;

namespace InventorySystemAPI.Models
{
    public class Order
    {
        // 訂單編號(主鍵)
        public int Id { get; set; }

        // 客戶姓名
        public string CustomerName { get; set; } = string.Empty;

        // 客戶電話
        public string? CustomerPhone { get; set; }

        // 訂單狀態：Pending(待處理), Completed(已完成), Cancelled(已取消)
        public string Status { get; set; } = "Pending";

        // 訂單總金額
        public decimal TotalAmount { get; set; }

        // 備註
        public string? Remarks { get; set; }

        // 訂單日期
        public DateTime OrderDate { get; set; } = DateTime.Now;

        // 導航屬性：訂單明細列表
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}