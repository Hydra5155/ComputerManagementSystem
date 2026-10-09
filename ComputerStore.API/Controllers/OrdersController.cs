using ComputerStore.Core.Entities;
using ComputerStore.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/orders - Lấy danh sách đơn hàng kèm chi tiết (được chiếu phẳng để tránh cycle loop)
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .OrderByDescending(o => o.OrderDate)
                .AsNoTracking()
                .Select(o => new
                {
                    o.OrderId,
                    o.CustomerName,
                    o.CustomerPhone,
                    DeliveryAddress = o.DeliveryAddress,
                    o.OrderDate,
                    o.TotalAmount,
                    o.Status,
                    OrderDetails = o.OrderDetails.Select(d => new
                    {
                        d.OrderDetailId,
                        d.OrderId,
                        d.ProductId,
                        ProductName = d.Product != null ? d.Product.Name : ("Sản phẩm #" + d.ProductId),
                        d.Quantity,
                        d.UnitPrice,
                        TotalPrice = d.Quantity * d.UnitPrice
                    }).ToList()
                })
                .ToListAsync();

            return Ok(orders);
        }

        // GET: api/orders/{id} - Lấy chi tiết 1 đơn hàng cụ thể
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .AsNoTracking()
                .Where(o => o.OrderId == id)
                .Select(o => new
                {
                    o.OrderId,
                    o.CustomerName,
                    o.CustomerPhone,
                    DeliveryAddress = o.DeliveryAddress,
                    o.OrderDate,
                    o.TotalAmount,
                    o.Status,
                    OrderDetails = o.OrderDetails.Select(d => new
                    {
                        d.OrderDetailId,
                        d.OrderId,
                        d.ProductId,
                        ProductName = d.Product != null ? d.Product.Name : ("Sản phẩm #" + d.ProductId),
                        d.Quantity,
                        d.UnitPrice,
                        TotalPrice = d.Quantity * d.UnitPrice
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound($"Không tìm thấy đơn hàng #{id}");
            }

            return Ok(order);
        }

        // POST: api/orders - Đặt hàng với Transaction an toàn và trừ tồn kho
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto request)
        {
            if (request == null || request.OrderDetails == null || !request.OrderDetails.Any())
            {
                return BadRequest("Thông tin đơn hàng không hợp lệ hoặc giỏ hàng trống.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                decimal calculatedTotal = 0;
                var detailsToInsert = new List<OrderDetail>();

                foreach (var detail in request.OrderDetails)
                {
                    var product = await _context.Products.FindAsync(detail.ProductId);
                    if (product == null)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest($"Sản phẩm có mã #{detail.ProductId} không tồn tại.");
                    }

                    if (product.StockQuantity < detail.Quantity)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest($"Sản phẩm '{product.Name}' chỉ còn {product.StockQuantity} máy trong kho.");
                    }

                    // Trừ tồn kho trong DB
                    product.StockQuantity -= detail.Quantity;

                    decimal unitPrice = product.Price;
                    calculatedTotal += unitPrice * detail.Quantity;

                    detailsToInsert.Add(new OrderDetail
                    {
                        ProductId = detail.ProductId,
                        Quantity = detail.Quantity,
                        UnitPrice = unitPrice
                    });
                }

                var newOrder = new Order
                {
                    CustomerName = request.CustomerName?.Trim() ?? string.Empty,
                    CustomerPhone = request.CustomerPhone?.Trim() ?? string.Empty,
                    DeliveryAddress = (request.DeliveryAddress ?? request.CustomerAddress ?? string.Empty).Trim(),
                    OrderDate = DateTime.Now,
                    TotalAmount = calculatedTotal,
                    Status = "Chờ xử lý",
                    OrderDetails = detailsToInsert
                };

                _context.Orders.Add(newOrder);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new
                {
                    Message = "Đặt hàng thành công!",
                    OrderId = newOrder.OrderId,
                    TotalAmount = newOrder.TotalAmount
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Lỗi xử lý giao dịch đơn hàng: {ex.Message}");
            }
        }

        // PUT: api/orders/{id}/status - Cập nhật trạng thái và hoàn kho nếu hủy đơn
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatusUpdateDto dto)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound("Không tìm thấy đơn hàng cần cập nhật.");
            }

            // Nếu hủy đơn hàng thì hoàn trả lại số lượng máy vào kho
            if (dto.Status == "Đã hủy" && order.Status != "Đã hủy")
            {
                foreach (var detail in order.OrderDetails)
                {
                    var product = await _context.Products.FindAsync(detail.ProductId);
                    if (product != null)
                    {
                        product.StockQuantity += detail.Quantity;
                    }
                }
            }

            order.Status = dto.Status;
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Đã cập nhật trạng thái đơn hàng #{id} thành '{dto.Status}'" });
        }
    }

    // Các DTOs phục vụ nhận dữ liệu đầu vào cho Controller
    public class OrderCreateDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string? DeliveryAddress { get; set; }
        public string? CustomerAddress { get; set; }
        public List<OrderDetailCreateDto> OrderDetails { get; set; } = new();
    }

    public class OrderDetailCreateDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderStatusUpdateDto
    {
        public string Status { get; set; } = string.Empty;
    }
}