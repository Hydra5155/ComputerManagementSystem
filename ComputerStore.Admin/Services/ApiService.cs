using ComputerStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputerStore.Admin.Services
{
    public class ApiService
    {
        private readonly HttpClient _client;
        private const string BaseUrl = "https://localhost:7275/api/";

        public ApiService()
        {
            // Bỏ qua kiểm tra SSL cho môi trường dev local
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            _client = new HttpClient(handler) { BaseAddress = new Uri(BaseUrl) };
        }

        // Lấy danh mục sản phẩm
        public async Task<List<Category>> GetCategoriesAsync()
        {
            try
            {
                var response = await _client.GetAsync("categories");
                if (!response.IsSuccessStatusCode)
                {
                    return new List<Category>();
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Category>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Category>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối khi lấy danh mục: {ex.Message}", "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Category>();
            }
        }

        // Lấy danh sách sản phẩm
        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                var response = await _client.GetAsync("products");
                if (!response.IsSuccessStatusCode)
                {
                    var err = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Không lấy được danh sách sản phẩm ({response.StatusCode}): {err}", "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return new List<Product>();
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Product>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối khi lấy sản phẩm: {ex.Message}", "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Product>();
            }
        }

        // Thêm sản phẩm mới kèm hiển thị chi tiết mã lỗi nếu thất bại
        public async Task<bool> AddProductAsync(Product product)
        {
            try
            {
                var json = JsonSerializer.Serialize(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("products", content);

                if (!response.IsSuccessStatusCode)
                {
                    var err = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi từ Server ({response.StatusCode}): {err}", "Lỗi API khi thêm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối khi thêm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Xóa sản phẩm
        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                var response = await _client.DeleteAsync($"products/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    var err = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi xóa sản phẩm ({response.StatusCode}): {err}", "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối khi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Lấy danh sách tất cả đơn hàng từ API
        public async Task<List<Order>> GetOrdersAsync()
        {
            try
            {
                var response = await _client.GetAsync("orders");
                if (!response.IsSuccessStatusCode)
                {
                    var err = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Không lấy được danh sách đơn hàng ({response.StatusCode}): {err}", "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return new List<Order>();
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Order>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Order>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối khi lấy đơn hàng: {ex.Message}", "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Order>();
            }
        }

        // Cập nhật trạng thái đơn hàng (Chờ xử lý, Đang giao, Đã hoàn thành, Đã hủy)
        public async Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            try
            {
                var updatePayload = new { Status = newStatus };
                var json = JsonSerializer.Serialize(updatePayload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"orders/{orderId}/status", content);
                if (!response.IsSuccessStatusCode)
                {
                    var err = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi cập nhật trạng thái ({response.StatusCode}): {err}", "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối khi cập nhật đơn hàng: {ex.Message}", "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}