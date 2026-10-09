using ComputerStore.Admin.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComputerStore.Admin
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            SetupCyberDashboard();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void SetupCyberDashboard()
        {
            // 1. Cấu hình Form
            this.Text = "DTBStore - Trung Tâm Quản Trị Hệ Thống Phần Cứng";
            this.Size = new Size(1180, 720);
            this.MinimumSize = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(8, 12, 13);
            this.ForeColor = Color.White;

            // Xóa sạch toàn bộ control thừa từ Designer
            this.Controls.Clear();

            // 2. Dùng TableLayoutPanel chia làm 2 cột tuyệt đối (Không bao giờ bị đè lấn)
            TableLayoutPanel rootLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.FromArgb(8, 12, 13),
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            // Cột 0: Rộng cố định 250px | Cột 1: Tự động chiếm 100% còn lại
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // ==================== CỘT TRÁI: SIDEBAR ====================
            Panel panelSidebar = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(14, 21, 22),
                Padding = new Padding(10),
                Margin = new Padding(0)
            };

            Label lblBrand = new Label
            {
                Text = "DTBSTORE ADMIN",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(93, 242, 214),
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };
            panelSidebar.Controls.Add(lblBrand);

            Button btnOrders = CreateMenuButton("🛒  QUẢN LÝ ĐƠN HÀNG");
            btnOrders.Top = 80;
            btnOrders.Click += (s, e) =>
            {
                var orderForm = new OrderManagementForm();
                orderForm.ShowDialog();
            };
            panelSidebar.Controls.Add(btnOrders);

            Button btnProducts = CreateMenuButton("💻  QUẢN LÝ SẢN PHẨM");
            btnProducts.Top = 145;
            btnProducts.Click += (s, e) =>
            {
                var productForm = new ProductManagementForm();
                productForm.ShowDialog();
            };
            panelSidebar.Controls.Add(btnProducts);

            Button btnLogout = CreateMenuButton("🚪  ĐĂNG XUẤT");
            btnLogout.Top = 210;
            btnLogout.ForeColor = Color.FromArgb(248, 113, 113);
            btnLogout.Click += (s, e) =>
            {
                var confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn đăng xuất phiên làm việc?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    this.Hide();
                    var login = new LoginForm();
                    login.ShowDialog();
                    this.Close();
                }
            };
            panelSidebar.Controls.Add(btnLogout);

            rootLayout.Controls.Add(panelSidebar, 0, 0);

            // ==================== CỘT PHẢI: NỘI DUNG CHÍNH ====================
            Panel panelContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(8, 12, 13),
                Padding = new Padding(30, 20, 20, 20),
                Margin = new Padding(0),
                AutoScroll = true
            };

            Label lblWelcome = new Label
            {
                Text = "TRUNG TÂM KIỂM SOÁT ĐƠN HÀNG && KHO MÁY TÍNH",
                UseMnemonic = false, // Giữ nguyên dấu & không bị nuốt chữ
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = Color.FromArgb(93, 242, 214),
                AutoSize = true,
                Location = new Point(30, 20)
            };
            panelContent.Controls.Add(lblWelcome);

            Label lblSub = new Label
            {
                Text = "Dữ liệu được đồng bộ hóa tức thời qua Web API từ DTBStore E-Commerce.",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(160, 175, 175),
                AutoSize = true,
                Location = new Point(30, 60)
            };
            panelContent.Controls.Add(lblSub);

            // Khung chứa các thẻ
            FlowLayoutPanel flowCards = new FlowLayoutPanel
            {
                Location = new Point(30, 110),
                Size = new Size(820, 450),
                BackColor = Color.Transparent,
                AutoScroll = true
            };

            // Card 1: Quản lý Đơn hàng
            Panel cardOrders = CreateInfoCard(
                "ĐƠN HÀNG TRỰC TUYẾN",
                "Xử lý, duyệt đơn và cập nhật vận chuyển khách đặt từ website.",
                "MỞ QUẢN LÝ ĐƠN"
            );
            Button btnCardOrder = cardOrders.Controls["actionBtn"] as Button;
            if (btnCardOrder != null)
            {
                btnCardOrder.Click += (s, e) =>
                {
                    var orderForm = new OrderManagementForm();
                    orderForm.ShowDialog();
                };
            }
            flowCards.Controls.Add(cardOrders);

            // Card 2: Kho phần cứng
            Panel cardProducts = CreateInfoCard(
                "KHO LINH KIỆN && LAPTOP",
                "Quản lý danh sách mẫu mã, cập nhật giá bán, số lượng tồn kho.",
                "MỞ QUẢN LÝ KHO"
            );
            Button btnCardProd = cardProducts.Controls["actionBtn"] as Button;
            if (btnCardProd != null)
            {
                btnCardProd.Click += (s, e) =>
                {
                    var productForm = new ProductManagementForm();
                    productForm.ShowDialog();
                };
            }
            flowCards.Controls.Add(cardProducts);

            panelContent.Controls.Add(flowCards);

            rootLayout.Controls.Add(panelContent, 1, 0);

            // Đưa bảng chia cột duy nhất vào Form
            this.Controls.Add(rootLayout);
        }

        private Button CreateMenuButton(string text)
        {
            var btn = new Button
            {
                Text = text,
                Width = 230,
                Height = 48,
                Left = 10,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(20, 31, 33),
                ForeColor = Color.FromArgb(93, 242, 214),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderColor = Color.FromArgb(30, 69, 62);
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 121, 107);

            return btn;
        }

        private Panel CreateInfoCard(string title, string desc, string btnText)
        {
            Panel card = new Panel
            {
                Width = 350,
                Height = 220,
                Margin = new Padding(0, 0, 25, 20),
                BackColor = Color.FromArgb(14, 21, 22),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(18)
            };

            Label lblTitle = new Label
            {
                Text = title,
                UseMnemonic = false,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(93, 242, 214),
                Dock = DockStyle.Top,
                Height = 35
            };

            Label lblDesc = new Label
            {
                Text = desc,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(200, 215, 215),
                Dock = DockStyle.Top,
                Height = 75
            };

            Button btnAction = new Button
            {
                Name = "actionBtn",
                Text = btnText,
                Dock = DockStyle.Bottom,
                Height = 45,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 121, 107),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnAction.FlatAppearance.BorderColor = Color.FromArgb(93, 242, 214);

            card.Controls.Add(btnAction);
            card.Controls.Add(lblDesc);
            card.Controls.Add(lblTitle);

            return card;
        }
    }
}