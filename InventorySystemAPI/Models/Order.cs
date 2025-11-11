using System.ComponentModel.DataAnnotations;

namespace InventorySystemAPI.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "客戶姓名為必填")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "客戶姓名長度必須在 1-50 字之間")]
        public string CustomerName { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "客戶電話為必填")]
        [Phone(ErrorMessage = "電話格式不正確")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "電話號碼長度必須在 1-20 字之間")]
        public string CustomerPhone { get; set; } = null!;

        public string Status { get; set; } = "Pending";

        public decimal TotalAmount { get; set; }

        [StringLength(500, ErrorMessage = "備註不能超過 500 字")]
        public string? Remarks { get; set; }

        public DateTime OrderDate { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();
    }
}