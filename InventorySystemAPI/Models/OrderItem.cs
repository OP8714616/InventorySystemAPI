namespace InventorySystemAPI.Models
{
    public class OrderItem
    {
        // 訂單明細編號(主鍵)
        public int Id { get; set; }

        // 訂單編號(外鍵)
        public int OrderId { get; set; }

        // 商品編號(外鍵)
        public int ProductId { get; set; }

        // 商品單價(下單時的價格)
        public decimal UnitPrice { get; set; }

        // 購買數量
        public int Quantity { get; set; }

        // 小計(單價 × 數量)
        public decimal Subtotal { get; set; }

        // 導航屬性：關聯到訂單
        public Order? Order { get; set; }

        // 導航屬性：關聯到商品
        public Product? Product { get; set; }
    }
}