using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComputerStore.Core.Entities;
using ComputerStore.Infrastructure.Data;

namespace ComputerStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/products (Bộ lọc đa điều kiện chặt chẽ)
        [HttpGet]
        public async Task<IActionResult> GetProducts(
            [FromQuery] string? query,
            [FromQuery] int? categoryId,
            [FromQuery] string? brand,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] string? sortBy)
        {
            var q = _context.Products.Include(p => p.Category).AsNoTracking().AsQueryable();

            // 1. Ràng buộc từ khóa (tìm cả trong tên, mô tả và cấu hình)
            if (!string.IsNullOrWhiteSpace(query))
            {
                var keyword = query.Trim().ToLower();
                q = q.Where(p => p.Name.ToLower().Contains(keyword) ||
                                (p.Specifications != null && p.Specifications.ToLower().Contains(keyword)));
            }

            // 2. Ràng buộc Danh mục
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                q = q.Where(p => p.CategoryId == categoryId.Value);
            }

            // 3. Ràng buộc Thương hiệu (Brand)
            if (!string.IsNullOrWhiteSpace(brand))
            {
                var brandTerm = brand.Trim().ToLower();
                q = q.Where(p => p.Name.ToLower().Contains(brandTerm));
            }

            // 4. Ràng buộc chặt chẽ khoảng giá
            if (minPrice.HasValue && minPrice.Value < 0) minPrice = 0;
            if (maxPrice.HasValue && maxPrice.Value < 0) maxPrice = 0;

            if (minPrice.HasValue && maxPrice.HasValue && minPrice.Value > maxPrice.Value)
            {
                // Nếu người dùng nhập min > max -> tự động hoán đổi để không bị rỗng kết quả
                var temp = minPrice;
                minPrice = maxPrice;
                maxPrice = temp;
            }

            if (minPrice.HasValue && minPrice.Value > 0)
            {
                q = q.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue && maxPrice.Value > 0)
            {
                q = q.Where(p => p.Price <= maxPrice.Value);
            }

            // 5. Sắp xếp
            q = sortBy switch
            {
                "price_asc" => q.OrderBy(p => p.Price),
                "price_desc" => q.OrderByDescending(p => p.Price),
                "name_asc" => q.OrderBy(p => p.Name),
                _ => q.OrderByDescending(p => p.ProductId)
            };

            var list = await q.ToListAsync();
            return Ok(list);
        }

        // GET: api/products/suggest?term=... (Tăng lên 15 sản phẩm để cuộn mượt mà)
        [HttpGet("suggest")]
        public async Task<IActionResult> Suggest([FromQuery] string? term)
        {
            if (string.IsNullOrWhiteSpace(term) || term.Trim().Length < 1)
            {
                return Ok(new List<object>());
            }

            var keyword = term.Trim().ToLower();
            var suggestions = await _context.Products
                .AsNoTracking()
                .Where(p => p.Name.ToLower().Contains(keyword) ||
                           (p.Specifications != null && p.Specifications.ToLower().Contains(keyword)))
                .Take(15) // Tăng từ 6 lên 15 sản phẩm để cuộn thoải mái
                .Select(p => new
                {
                    p.ProductId,
                    p.Name,
                    p.Price,
                    p.ImageUrl
                })
                .ToListAsync();

            return Ok(suggestions);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null) return NotFound();
            return Ok(product);
        }
    }
}