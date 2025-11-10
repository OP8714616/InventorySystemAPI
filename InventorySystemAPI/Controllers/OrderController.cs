using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventorySystemAPI.Data;
using InventorySystemAPI.Models;

namespace InventorySystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound(new { message = "訂單不存在" });
            }

            return order;
        }

        // POST: api/Order
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var item in order.OrderItems)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);

                    if (product == null)
                    {
                        return BadRequest(new { message = $"商品 ID {item.ProductId} 不存在" });
                    }

                    if (product.Stock < item.Quantity)
                    {
                        return BadRequest(new
                        {
                            message = $"商品 {product.Name} 庫存不足！目前庫存：{product.Stock}，需求：{item.Quantity}"
                        });
                    }

                    item.UnitPrice = product.Price;
                    item.Subtotal = item.UnitPrice * item.Quantity;
                }

                order.TotalAmount = order.OrderItems.Sum(i => i.Subtotal);
                order.OrderDate = DateTime.Now;
                order.Status = "Pending";

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                foreach (var item in order.OrderItems)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    product!.Stock -= item.Quantity;

                    var inventory = new Inventory
                    {
                        ProductId = item.ProductId,
                        Type = "Out",
                        Quantity = -item.Quantity,
                        Remarks = $"訂單 #{order.Id} 出貨",
                        CreatedAt = DateTime.Now
                    };
                    _context.Inventories.Add(inventory);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var createdOrder = await _context.Orders
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(o => o.Id == order.Id);

                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, createdOrder);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = "建立訂單失敗", error = ex.Message });
            }
        }

        // PUT: api/Order/5/cancel
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound(new { message = "訂單不存在" });
            }

            if (order.Status == "Cancelled")
            {
                return BadRequest(new { message = "訂單已經取消過了" });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var item in order.OrderItems)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product != null)
                    {
                        product.Stock += item.Quantity;

                        var inventory = new Inventory
                        {
                            ProductId = item.ProductId,
                            Type = "In",
                            Quantity = item.Quantity,
                            Remarks = $"訂單 #{order.Id} 取消退回",
                            CreatedAt = DateTime.Now
                        };
                        _context.Inventories.Add(inventory);
                    }
                }

                order.Status = "Cancelled";
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { message = "訂單已取消，庫存已恢復", order });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = "取消訂單失敗", error = ex.Message });
            }
        }
    }
}