using ComputerStore.Admin.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputerStore.Admin.Forms
{
    public partial class OrderManagementForm : Form
    {
        private readonly HttpClient _httpClient;
        private List<OrderDto> _orders = new();
        private readonly string[] _allStatuses = { "Tất cả", "Chờ xử lý", "Đang giao", "Đã hoàn thành", "Đã hủy" };

        public OrderManagementForm()
        {
            InitializeComponent();

            // Đổi URL này cho khớp với cổng chạy thực tế của ComputerStore.API
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5293/api/")
            };
        }

        private async void OrderManagementForm_Load(object sender, EventArgs e)
        {
            ApplyCyberDarkTheme();
            InitFilterControls();
            await FetchOrdersAsync();
        }

        private void InitFilterControls()
        {
            cboStatusFilter.Items.Clear();
            cboStatusFilter.Items.AddRange(_allStatuses);
            cboStatusFilter.SelectedIndex = 0;

            cboNewStatus.Items.Clear();
            cboNewStatus.Items.AddRange(new string[] { "Chờ xử lý", "Đang giao", "Đã hoàn thành", "Đã hủy" });
            cboNewStatus.SelectedIndex = 0;
        }

        private void ApplyCyberDarkTheme()
        {
            // Định dạng DataGridView theo chuẩn Cyber Dark
            void FormatGrid(DataGridView g)
            {
                g.EnableHeadersVisualStyles = false;
                g.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 61, 53);
                g.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(93, 242, 214);
                g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                g.DefaultCellStyle.BackColor = Color.FromArgb(14, 21, 22);
                g.DefaultCellStyle.ForeColor = Color.White;
                g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 121, 107);
                g.DefaultCellStyle.SelectionForeColor = Color.White;
                g.BorderStyle = BorderStyle.None;
                g.GridColor = Color.FromArgb(30, 69, 62);
            }

            FormatGrid(dgvOrders);
            FormatGrid(dgvDetails);
        }

        private async Task FetchOrdersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("orders");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    _orders = JsonSerializer.Deserialize<List<OrderDto>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<OrderDto>();

                    RenderOrders();
                }
                else
                {
                    MessageBox.Show("Không thể tải danh sách đơn từ API.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối API: {ex.Message}\nVui lòng kiểm tra ComputerStore.API đã chạy chưa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RenderOrders()
        {
            string selected = cboStatusFilter.SelectedItem?.ToString() ?? "Tất cả";
            var query = selected == "Tất cả"
                ? _orders
                : _orders.Where(o => string.Equals(o.Status, selected, StringComparison.OrdinalIgnoreCase)).ToList();

            dgvOrders.DataSource = null;
            dgvOrders.DataSource = query.Select(o => new
            {
                Mã_Đơn = o.OrderId,
                Khách_Hàng = o.CustomerName,
                Số_Điện_Thoại = o.CustomerPhone,
                Địa_Chỉ = o.FinalAddress,
                Ngày_Đặt = o.OrderDate.ToString("dd/MM/yyyy HH:mm"),
                Tổng_Tiền = o.TotalAmount.ToString("N0") + " đ",
                Trạng_Thái = o.Status
            }).ToList();

            if (dgvOrders.Rows.Count > 0)
            {
                dgvOrders.Rows[0].Selected = true;
                DisplayDetails(query.First());
            }
            else
            {
                dgvDetails.DataSource = null;
                lblDetailsHeader.Text = "CHI TIẾT MẶT HÀNG TRONG ĐƠN:";
            }
        }

        private void dgvOrders_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvOrders.CurrentRow == null) return;

            int orderId = Convert.ToInt32(dgvOrders.CurrentRow.Cells["Mã_Đơn"].Value);
            var order = _orders.FirstOrDefault(x => x.OrderId == orderId);
            if (order != null)
            {
                DisplayDetails(order);
                cboNewStatus.SelectedItem = order.Status;
            }
        }

        private void DisplayDetails(OrderDto order)
        {
            lblDetailsHeader.Text = $"CHI TIẾT MẶT HÀNG - ĐƠN HÀNG #{order.OrderId} ({order.CustomerName})";

            dgvDetails.DataSource = null;
            if (order.OrderDetails != null && order.OrderDetails.Any())
            {
                dgvDetails.DataSource = order.OrderDetails.Select(d => new
                {
                    Mã_SP = d.ProductId,
                    Tên_Sản_Phẩm = string.IsNullOrEmpty(d.ProductName) ? $"Sản phẩm #{d.ProductId}" : d.ProductName,
                    Số_Lượng = d.Quantity,
                    Đơn_Giá = d.UnitPrice.ToString("N0") + " đ",
                    Thành_Tiền = d.TotalPrice.ToString("N0") + " đ"
                }).ToList();
            }
        }

        private async void btnSaveStatus_Click(object sender, EventArgs e)
        {
            if (dgvOrders.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn đơn hàng cần cập nhật!", "Nhắc nhở", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int orderId = Convert.ToInt32(dgvOrders.CurrentRow.Cells["Mã_Đơn"].Value);
            string newStatus = cboNewStatus.SelectedItem?.ToString() ?? "Chờ xử lý";

            try
            {
                var payload = new { Status = newStatus };
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"orders/{orderId}/status", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Cập nhật đơn hàng #{orderId} thành '{newStatus}' thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await FetchOrdersAsync();
                }
                else
                {
                    MessageBox.Show("API không cập nhật được trạng thái.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            RenderOrders();
        }

        private async void btnReload_Click(object sender, EventArgs e)
        {
            await FetchOrdersAsync();
        }

        private void dgvOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}