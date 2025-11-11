using System.ComponentModel.DataAnnotations;

namespace InventorySystemAPI.Models
{
    public class Inventory
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "商品 ID 為必填")]
        public int ProductId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "類型為必填")]
        [RegularExpression("^(In|Out)$", ErrorMessage = "類型只能是 In 或 Out")]
        public string Type { get; set; } = null!;

        [Required(ErrorMessage = "數量為必填")]
        public int Quantity { get; set; }

        [StringLength(500, ErrorMessage = "備註不能超過 500 字")]
        public string? Remarks { get; set; }

        public DateTime CreatedAt { get; set; }

        public Product? Product { get; set; }
    }
}