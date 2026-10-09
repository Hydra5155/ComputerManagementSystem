using ComputerStore.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ComputerStore.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Product?search=...&priceRange=under15
        [HttpGet]
        public async Task<IActionResult> Index(string? search, string? priceRange)
        {
            List<Product> products = new();
            try
            {
                var client = _httpClientFactory.CreateClient("StoreAPI");
                string url = "products";
                if (!string.IsNullOrWhiteSpace(search))
                {
                    url += $"?search={Uri.EscapeDataString(search.Trim())}";
                }

                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    products = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<Product>();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Lỗi kết nối API: {ex.Message}";
            }

            if (!string.IsNullOrEmpty(priceRange))
            {
                switch (priceRange)
                {
                    case "under15":
                        products = products.Where(p => p.Price < 15000000).ToList();
                        break;
                    case "15to25":
                        products = products.Where(p => p.Price >= 15000000 && p.Price <= 25000000).ToList();
                        break;
                    case "above25":
                        products = products.Where(p => p.Price > 25000000).ToList();
                        break;
                }
            }

            ViewBag.CurrentSearch = search;
            ViewBag.CurrentPriceRange = priceRange;

            return View(products);
        }

        // GET: /Product/Detail/{id}
        [HttpGet("Product/Detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            Product? product = null;
            try
            {
                var client = _httpClientFactory.CreateClient("StoreAPI");

                // Thử endpoint lấy chi tiết
                var response = await client.GetAsync($"products/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    product = JsonSerializer.Deserialize<Product>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    // Fallback lấy toàn bộ danh sách để tìm
                    var listResponse = await client.GetAsync("products");
                    if (listResponse.IsSuccessStatusCode)
                    {
                        var json = await listResponse.Content.ReadAsStringAsync();
                        var allProducts = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }) ?? new List<Product>();

                        product = allProducts.FirstOrDefault(p => {
                            var pIdProp = p.GetType().GetProperty("ProductId") ?? p.GetType().GetProperty("Id");
                            return pIdProp != null && Convert.ToInt32(pIdProp.GetValue(p)) == id;
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Lỗi kết nối: {ex.Message}";
            }

            if (product == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }
    }
}