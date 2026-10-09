using ComputerStore.Core.Entities;
using ComputerStore.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ComputerStore.Web.Controllers
{
    // DTO lưu trữ trong Session đảm bảo độc lập và an toàn dữ liệu
    public class CartDtoItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; }
    }

    public class CartController : Controller
    {
        private const string CartSessionKey = "DTB_CartSession_v3";
        private readonly IHttpClientFactory _httpClientFactory;

        public CartController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Cart
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var dtoList = GetDtoList();

            // Cơ chế tự khôi phục: Nếu món hàng bị thiếu giá hoặc tên mặc định, tra cứu API để nạp lại
            if (dtoList.Any(x => x.Price <= 0 || string.IsNullOrWhiteSpace(x.ProductName) || x.ProductName == "Laptop Gaming"))
            {
                try
                {
                    var client = _httpClientFactory.CreateClient("StoreAPI");
                    var response = await client.GetAsync("products");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var allProducts = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if (allProducts != null)
                        {
                            bool updated = false;
                            foreach (var item in dtoList.Where(x => x.Price <= 0 || x.ProductName == "Laptop Gaming"))
                            {
                                var prod = allProducts.FirstOrDefault(p => p.ProductId == item.ProductId);
                                if (prod != null)
                                {
                                    item.ProductName = prod.Name;
                                    item.Price = prod.Price;
                                    if (!string.IsNullOrEmpty(prod.ImageUrl))
                                    {
                                        item.ImageUrl = prod.ImageUrl;
                                    }
                                    updated = true;
                                }
                            }
                            if (updated) SaveDtoList(dtoList);
                        }
                    }
                }
                catch { }
            }

            var cartItems = ConvertToCartItems(dtoList);
            return View(cartItems);
        }

        // THÊM SẢN PHẨM VÀO GIỎ HÀNG
        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> AddToCart(int? productId, int? id, string? productName, decimal? price, string? imageUrl, int quantity = 1)
        {
            int targetId = productId ?? id ?? 0;
            if (targetId <= 0) return RedirectToAction("Index", "Product");

            decimal finalPrice = price ?? 0m;
            string finalName = productName ?? "";
            string finalImg = imageUrl ?? "";

            // Nếu thiếu tên hoặc giá, gọi API để lấy dữ liệu thực tế của sản phẩm
            if (finalPrice <= 0 || string.IsNullOrWhiteSpace(finalName))
            {
                try
                {
                    var client = _httpClientFactory.CreateClient("StoreAPI");
                    var listResp = await client.GetAsync("products");
                    if (listResp.IsSuccessStatusCode)
                    {
                        var json = await listResp.Content.ReadAsStringAsync();
                        var allProducts = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        var prod = allProducts?.FirstOrDefault(p => p.ProductId == targetId);
                        if (prod != null)
                        {
                            finalName = prod.Name;
                            finalPrice = prod.Price;
                            finalImg = prod.ImageUrl ?? "";
                        }
                    }
                }
                catch { }
            }

            if (string.IsNullOrWhiteSpace(finalName)) finalName = "Laptop Gaming";

            var dtoList = GetDtoList();
            var item = dtoList.FirstOrDefault(x => x.ProductId == targetId);

            if (item != null)
            {
                item.Quantity += (quantity > 0 ? quantity : 1);
                if (item.Price <= 0 && finalPrice > 0) item.Price = finalPrice;
                if (item.ProductName == "Laptop Gaming" && finalName != "Laptop Gaming") item.ProductName = finalName;
            }
            else
            {
                dtoList.Add(new CartDtoItem
                {
                    ProductId = targetId,
                    ProductName = finalName,
                    Price = finalPrice,
                    ImageUrl = finalImg,
                    Quantity = quantity > 0 ? quantity : 1
                });
            }

            SaveDtoList(dtoList);
            return RedirectToAction(nameof(Index));
        }

        // CẬP NHẬT SỐ LƯỢNG (TĂNG / GIẢM / SỬA)
        [HttpGet]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            if (productId <= 0) return RedirectToAction(nameof(Index));

            var dtoList = GetDtoList();
            var item = dtoList.FirstOrDefault(x => x.ProductId == productId);

            if (item != null)
            {
                if (quantity <= 0)
                {
                    dtoList.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }
                SaveDtoList(dtoList);
            }

            return RedirectToAction(nameof(Index));
        }

        // XÓA 1 MÓN HÀNG KHỎI GIỎ
        [HttpGet]
        public IActionResult RemoveFromCart(int productId)
        {
            if (productId <= 0) return RedirectToAction(nameof(Index));

            var dtoList = GetDtoList();
            var item = dtoList.FirstOrDefault(x => x.ProductId == productId);

            if (item != null)
            {
                dtoList.Remove(item);
                SaveDtoList(dtoList);
            }

            return RedirectToAction(nameof(Index));
        }

        // LÀM TRỐNG TOÀN BỘ GIỎ HÀNG
        [HttpGet]
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(CartSessionKey);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Cart/Checkout - Trang thanh toán
        [HttpGet]
        public IActionResult Checkout()
        {
            var dtoList = GetDtoList();
            if (!dtoList.Any()) return RedirectToAction(nameof(Index));

            var cartItems = ConvertToCartItems(dtoList);
            return View(cartItems);
        }

        // POST: /Cart/Checkout - LƯU ĐƠN VÀO HỆ THỐNG / DATABASE QUA WEB API
        [HttpPost]
        public async Task<IActionResult> Checkout(string customerName, string customerPhone, string shippingAddress)
        {
            var dtoList = GetDtoList();
            if (!dtoList.Any())
            {
                return RedirectToAction("Index", "Product");
            }

            try
            {
                var client = _httpClientFactory.CreateClient("StoreAPI");

                // Đóng gói Payload theo Entity Order và OrderDetail
                var orderPayload = new
                {
                    CustomerName = string.IsNullOrWhiteSpace(customerName) ? "Khách hàng" : customerName.Trim(),
                    CustomerPhone = string.IsNullOrWhiteSpace(customerPhone) ? "" : customerPhone.Trim(),
                    Phone = string.IsNullOrWhiteSpace(customerPhone) ? "" : customerPhone.Trim(),
                    ShippingAddress = string.IsNullOrWhiteSpace(shippingAddress) ? "" : shippingAddress.Trim(),
                    Address = string.IsNullOrWhiteSpace(shippingAddress) ? "" : shippingAddress.Trim(),
                    OrderDate = DateTime.Now,
                    TotalAmount = dtoList.Sum(x => x.Price * x.Quantity),
                    Status = "Chờ xử lý",
                    OrderDetails = dtoList.Select(item => new
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        UnitPrice = item.Price,
                        Price = item.Price,
                        TotalPrice = item.Price * item.Quantity
                    }).ToList()
                };

                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(orderPayload),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                // Gửi request POST tới endpoint orders của Web API
                var response = await client.PostAsync("orders", jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    // Fallback thử endpoint phụ nếu API dùng quy ước khác
                    await client.PostAsync("Orders/create", jsonContent);
                }

                // Xóa session giỏ hàng sau khi đặt thành công
                HttpContext.Session.Remove(CartSessionKey);
                TempData["SuccessMessage"] = "Đặt hàng thành công!";
                return RedirectToAction(nameof(OrderSuccess));
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Lỗi khi kết nối API đặt hàng: {ex.Message}";
                var cartItems = ConvertToCartItems(dtoList);
                return View(cartItems);
            }
        }

        // GET: /Cart/OrderSuccess - Trang thông báo thành công
        [HttpGet]
        public IActionResult OrderSuccess()
        {
            return View();
        }

        // --- HÀM PHỤ TRỢ NỘI BỘ ---
        private List<CartDtoItem> GetDtoList()
        {
            var json = HttpContext.Session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(json)) return new List<CartDtoItem>();

            try
            {
                return JsonSerializer.Deserialize<List<CartDtoItem>>(json) ?? new List<CartDtoItem>();
            }
            catch
            {
                return new List<CartDtoItem>();
            }
        }

        private void SaveDtoList(List<CartDtoItem> list)
        {
            HttpContext.Session.SetString(CartSessionKey, JsonSerializer.Serialize(list));
        }

        // Ánh xạ sang List<CartItem> mà View yêu cầu
        private List<CartItem> ConvertToCartItems(List<CartDtoItem> dtoList)
        {
            var result = new List<CartItem>();
            var itemType = typeof(CartItem);

            foreach (var dto in dtoList)
            {
                var item = new CartItem();

                itemType.GetProperty("ProductId")?.SetValue(item, dto.ProductId);
                itemType.GetProperty("Id")?.SetValue(item, dto.ProductId);
                itemType.GetProperty("ProductName")?.SetValue(item, dto.ProductName);
                itemType.GetProperty("Name")?.SetValue(item, dto.ProductName);
                itemType.GetProperty("Price")?.SetValue(item, dto.Price);
                itemType.GetProperty("UnitPrice")?.SetValue(item, dto.Price);
                itemType.GetProperty("ImageUrl")?.SetValue(item, dto.ImageUrl);
                itemType.GetProperty("Image")?.SetValue(item, dto.ImageUrl);
                itemType.GetProperty("Quantity")?.SetValue(item, dto.Quantity);

                var pProp = itemType.GetProperty("Product");
                if (pProp != null)
                {
                    var prod = new Product
                    {
                        ProductId = dto.ProductId,
                        Name = dto.ProductName,
                        Price = dto.Price,
                        ImageUrl = dto.ImageUrl
                    };
                    pProp.SetValue(item, prod);
                }

                result.Add(item);
            }

            return result;
        }
    }
}