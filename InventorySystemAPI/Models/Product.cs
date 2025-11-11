using System.ComponentModel.DataAnnotations;

namespace InventorySystemAPI.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "商品名稱為必填")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "商品名稱長度必須在 1-100 字之間")]
        public string Name { get; set; } = null!;

        [StringLength(500, ErrorMessage = "商品描述不能超過 500 字")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "價格為必填")]
        [Range(0.01, 999999.99, ErrorMessage = "價格必須在 0.01 到 999999.99 之間")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "庫存為必填")]
        [Range(0, int.MaxValue, ErrorMessage = "庫存不能為負數")]
        public int Stock { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}