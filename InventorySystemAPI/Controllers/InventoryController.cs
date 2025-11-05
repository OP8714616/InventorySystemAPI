using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventorySystemAPI.Data;
using InventorySystemAPI.Models;

namespace InventorySystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InventoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Inventory
        // 查詢所有庫存記錄
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetInventories()
        {
            return await _context.Inventories
                .Include(i => i.Product)  // 包含關聯的商品資料
                .ToListAsync();
        }

        // GET: api/Inventory/product/5
        // 查詢特定商品的庫存記錄
        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetInventoriesByProduct(int productId)
        {
            var inventories = await _context.Inventories
                .Where(i => i.ProductId == productId)
                .Include(i => i.Product)
                .OrderByDescending(i => i.CreatedAt)  // 最新的在前面
                .ToListAsync();

            return inventories;
        }

        // POST: api/Inventory/in
        // 進貨（入庫）
        [HttpPost("in")]
        public async Task<ActionResult<Inventory>> StockIn(InventoryRequest request)
        {
            // 檢查商品是否存在
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                return NotFound($"找不到編號 {request.ProductId} 的商品");
            }

            // 建立庫存記錄
            var inventory = new Inventory
            {
                ProductId = request.ProductId,
                Type = "In",
                Quantity = request.Quantity,
                Remarks = request.Remarks,
                CreatedAt = DateTime.Now
            };

            _context.Inventories.Add(inventory);

            // 更新商品庫存
            product.Stock += request.Quantity;

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInventories), new { id = inventory.Id }, inventory);
        }

        // POST: api/Inventory/out
        // 出貨（出庫）
        [HttpPost("out")]
        public async Task<ActionResult<Inventory>> StockOut(InventoryRequest request)
        {
            // 檢查商品是否存在
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                return NotFound($"找不到編號 {request.ProductId} 的商品");
            }

            // 檢查庫存是否足夠
            if (product.Stock < request.Quantity)
            {
                return BadRequest($"庫存不足！目前庫存：{product.Stock}，要出貨：{request.Quantity}");
            }

            // 建立庫存記錄
            var inventory = new Inventory
            {
                ProductId = request.ProductId,
                Type = "Out",
                Quantity = -request.Quantity,  // 負數表示減少
                Remarks = request.Remarks,
                CreatedAt = DateTime.Now
            };

            _context.Inventories.Add(inventory);

            // 更新商品庫存
            product.Stock -= request.Quantity;

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInventories), new { id = inventory.Id }, inventory);
        }
    }

    // 請求的資料格式
    public class InventoryRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string? Remarks { get; set; }
    }
}