namespace ComputerStore.Admin
{
    partial class UserManagementForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlHeader = new System.Windows.Forms.Panel();
            txtSearch = new System.Windows.Forms.TextBox();
            lblSearch = new System.Windows.Forms.Label();
            lblTitle = new System.Windows.Forms.Label();
            pnlDetails = new System.Windows.Forms.Panel();
            btnViewOrders = new System.Windows.Forms.Button();
            btnDelete = new System.Windows.Forms.Button();
            btnSave = new System.Windows.Forms.Button();
            btnAddNew = new System.Windows.Forms.Button();
            btnResetPass = new System.Windows.Forms.Button();
            txtNewPassword = new System.Windows.Forms.TextBox();
            lblPassTitle = new System.Windows.Forms.Label();
            cboRole = new System.Windows.Forms.ComboBox();
            lblRole = new System.Windows.Forms.Label();
            txtEmail = new System.Windows.Forms.TextBox();
            lblEmail = new System.Windows.Forms.Label();
            txtPhone = new System.Windows.Forms.TextBox();
            lblPhone = new System.Windows.Forms.Label();
            txtFullName = new System.Windows.Forms.TextBox();
            lblFullName = new System.Windows.Forms.Label();
            txtUsername = new System.Windows.Forms.TextBox();
            lblUsername = new System.Windows.Forms.Label();
            lblDetailTitle = new System.Windows.Forms.Label();
            dgvUsers = new System.Windows.Forms.DataGridView();
            pnlHeader.SuspendLayout();
            pnlDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = System.Drawing.Color.FromArgb(14, 21, 22);
            pnlHeader.Controls.Add(txtSearch);
            pnlHeader.Controls.Add(lblSearch);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            pnlHeader.Size = new System.Drawing.Size(1150, 60);
            pnlHeader.TabIndex = 0;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = System.Drawing.Color.FromArgb(20, 31, 33);
            txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtSearch.Dock = System.Windows.Forms.DockStyle.Right;
            txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtSearch.ForeColor = System.Drawing.Color.White;
            txtSearch.Location = new System.Drawing.Point(910, 10);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Nhập tên, tài khoản, SĐT...";
            txtSearch.Size = new System.Drawing.Size(220, 25);
            txtSearch.TabIndex = 2;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Dock = System.Windows.Forms.DockStyle.Right;
            lblSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            lblSearch.ForeColor = System.Drawing.Color.FromArgb(141, 164, 164);
            lblSearch.Location = new System.Drawing.Point(846, 10);
            lblSearch.Name = "lblSearch";
            lblSearch.Padding = new System.Windows.Forms.Padding(0, 6, 5, 0);
            lblSearch.Size = new System.Drawing.Size(64, 23);
            lblSearch.TabIndex = 1;
            lblSearch.Text = "Tìm kiếm:";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(93, 242, 214);
            lblTitle.Location = new System.Drawing.Point(20, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(420, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "👤 QUẢN LÝ TÀI KHOẢN & PHÂN QUYỀN HỆ THỐNG";
            lblTitle.UseMnemonic = false;
            // 
            // pnlDetails
            // 
            pnlDetails.BackColor = System.Drawing.Color.FromArgb(14, 21, 22);
            pnlDetails.Controls.Add(btnViewOrders);
            pnlDetails.Controls.Add(btnDelete);
            pnlDetails.Controls.Add(btnSave);
            pnlDetails.Controls.Add(btnAddNew);
            pnlDetails.Controls.Add(btnResetPass);
            pnlDetails.Controls.Add(txtNewPassword);
            pnlDetails.Controls.Add(lblPassTitle);
            pnlDetails.Controls.Add(cboRole);
            pnlDetails.Controls.Add(lblRole);
            pnlDetails.Controls.Add(txtEmail);
            pnlDetails.Controls.Add(lblEmail);
            pnlDetails.Controls.Add(txtPhone);
            pnlDetails.Controls.Add(lblPhone);
            pnlDetails.Controls.Add(txtFullName);
            pnlDetails.Controls.Add(lblFullName);
            pnlDetails.Controls.Add(txtUsername);
            pnlDetails.Controls.Add(lblUsername);
            pnlDetails.Controls.Add(lblDetailTitle);
            pnlDetails.Dock = System.Windows.Forms.DockStyle.Right;
            pnlDetails.Location = new System.Drawing.Point(790, 60);
            pnlDetails.Name = "pnlDetails";
            pnlDetails.Padding = new System.Windows.Forms.Padding(20);
            pnlDetails.Size = new System.Drawing.Size(360, 640);
            pnlDetails.TabIndex = 1;
            // 
            // btnViewOrders
            // 
            btnViewOrders.BackColor = System.Drawing.Color.FromArgb(20, 61, 53);
            btnViewOrders.Cursor = System.Windows.Forms.Cursors.Hand;
            btnViewOrders.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(93, 242, 214);
            btnViewOrders.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnViewOrders.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnViewOrders.ForeColor = System.Drawing.Color.FromArgb(93, 242, 214);
            btnViewOrders.Location = new System.Drawing.Point(20, 580);
            btnViewOrders.Name = "btnViewOrders";
            btnViewOrders.Size = new System.Drawing.Size(320, 32);
            btnViewOrders.TabIndex = 16;
            btnViewOrders.Text = "📦 XEM LỊCH SỬ ĐƠN HÀNG CỦA USER NÀY";
            btnViewOrders.UseVisualStyleBackColor = false;
            btnViewOrders.Click += btnViewOrders_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = System.Drawing.Color.FromArgb(40, 20, 20);
            btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            btnDelete.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(248, 113, 113);
            btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnDelete.ForeColor = System.Drawing.Color.FromArgb(248, 113, 113);
            btnDelete.Location = new System.Drawing.Point(20, 540);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(320, 34);
            btnDelete.TabIndex = 15;
            btnDelete.Text = "🗑️ XÓA TÀI KHOẢN";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnSave
            // 
            btnSave.BackColor = System.Drawing.Color.FromArgb(0, 121, 107);
            btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            btnSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(93, 242, 214);
            btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSave.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnSave.ForeColor = System.Drawing.Color.White;
            btnSave.Location = new System.Drawing.Point(185, 492);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(155, 38);
            btnSave.TabIndex = 14;
            btnSave.Text = "💾 CẬP NHẬT";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnAddNew
            // 
            btnAddNew.BackColor = System.Drawing.Color.FromArgb(16, 78, 139);
            btnAddNew.Cursor = System.Windows.Forms.Cursors.Hand;
            btnAddNew.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(96, 165, 250);
            btnAddNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAddNew.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnAddNew.ForeColor = System.Drawing.Color.White;
            btnAddNew.Location = new System.Drawing.Point(20, 492);
            btnAddNew.Name = "btnAddNew";
            btnAddNew.Size = new System.Drawing.Size(155, 38);
            btnAddNew.TabIndex = 17;
            btnAddNew.Text = "➕ TẠO MỚI";
            btnAddNew.UseVisualStyleBackColor = false;
            btnAddNew.Click += btnAddNew_Click;
            // 
            // btnResetPass
            // 
            btnResetPass.BackColor = System.Drawing.Color.FromArgb(234, 179, 8);
            btnResetPass.Cursor = System.Windows.Forms.Cursors.Hand;
            btnResetPass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnResetPass.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnResetPass.ForeColor = System.Drawing.Color.Black;
            btnResetPass.Location = new System.Drawing.Point(240, 428);
            btnResetPass.Name = "btnResetPass";
            btnResetPass.Size = new System.Drawing.Size(100, 26);
            btnResetPass.TabIndex = 13;
            btnResetPass.Text = "Cấp lại";
            btnResetPass.UseVisualStyleBackColor = false;
            btnResetPass.Click += btnResetPass_Click;
            // 
            // txtNewPassword
            // 
            txtNewPassword.BackColor = System.Drawing.Color.FromArgb(20, 31, 33);
            txtNewPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtNewPassword.ForeColor = System.Drawing.Color.White;
            txtNewPassword.Location = new System.Drawing.Point(20, 430);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.PasswordChar = '•';
            txtNewPassword.Size = new System.Drawing.Size(210, 24);
            txtNewPassword.TabIndex = 12;
            // 
            // lblPassTitle
            // 
            lblPassTitle.AutoSize = true;
            lblPassTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblPassTitle.ForeColor = System.Drawing.Color.FromArgb(250, 204, 21);
            lblPassTitle.Location = new System.Drawing.Point(20, 408);
            lblPassTitle.Name = "lblPassTitle";
            lblPassTitle.Size = new System.Drawing.Size(147, 15);
            lblPassTitle.TabIndex = 11;
            lblPassTitle.Text = "CẤP LẠI MẬT KHẨU MỚI:";
            // 
            // cboRole
            // 
            cboRole.BackColor = System.Drawing.Color.FromArgb(20, 31, 33);
            cboRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboRole.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            cboRole.ForeColor = System.Drawing.Color.White;
            cboRole.FormattingEnabled = true;
            cboRole.Items.AddRange(new object[] {
            "1 - Quản trị viên (Admin)",
            "2 - Khách hàng (Customer)"});
            cboRole.Location = new System.Drawing.Point(20, 350);
            cboRole.Name = "cboRole";
            cboRole.Size = new System.Drawing.Size(320, 25);
            cboRole.TabIndex = 10;
            // 
            // lblRole
            // 
            lblRole.AutoSize = true;
            lblRole.ForeColor = System.Drawing.Color.FromArgb(141, 164, 164);
            lblRole.Location = new System.Drawing.Point(20, 330);
            lblRole.Name = "lblRole";
            lblRole.Size = new System.Drawing.Size(120, 17);
            lblRole.TabIndex = 9;
            lblRole.Text = "Vai trò phân quyền:";
            // 
            // txtEmail
            // 
            txtEmail.BackColor = System.Drawing.Color.FromArgb(20, 31, 33);
            txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtEmail.ForeColor = System.Drawing.Color.White;
            txtEmail.Location = new System.Drawing.Point(20, 285);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new System.Drawing.Size(320, 24);
            txtEmail.TabIndex = 8;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.ForeColor = System.Drawing.Color.FromArgb(141, 164, 164);
            lblEmail.Location = new System.Drawing.Point(20, 265);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new System.Drawing.Size(42, 17);
            lblEmail.TabIndex = 7;
            lblEmail.Text = "Email:";
            // 
            // txtPhone
            // 
            txtPhone.BackColor = System.Drawing.Color.FromArgb(20, 31, 33);
            txtPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtPhone.ForeColor = System.Drawing.Color.White;
            txtPhone.Location = new System.Drawing.Point(20, 220);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new System.Drawing.Size(320, 24);
            txtPhone.TabIndex = 6;
            // 
            // lblPhone
            // 
            lblPhone.AutoSize = true;
            lblPhone.ForeColor = System.Drawing.Color.FromArgb(141, 164, 164);
            lblPhone.Location = new System.Drawing.Point(20, 200);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new System.Drawing.Size(88, 17);
            lblPhone.TabIndex = 5;
            lblPhone.Text = "Số điện thoại:";
            // 
            // txtFullName
            // 
            txtFullName.BackColor = System.Drawing.Color.FromArgb(20, 31, 33);
            txtFullName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtFullName.ForeColor = System.Drawing.Color.White;
            txtFullName.Location = new System.Drawing.Point(20, 155);
            txtFullName.Name = "txtFullName";
            txtFullName.Size = new System.Drawing.Size(320, 24);
            txtFullName.TabIndex = 4;
            // 
            // lblFullName
            // 
            lblFullName.AutoSize = true;
            lblFullName.ForeColor = System.Drawing.Color.FromArgb(141, 164, 164);
            lblFullName.Location = new System.Drawing.Point(20, 135);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new System.Drawing.Size(67, 17);
            lblFullName.TabIndex = 3;
            lblFullName.Text = "Họ và tên:";
            // 
            // txtUsername
            // 
            txtUsername.BackColor = System.Drawing.Color.FromArgb(20, 31, 33);
            txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtUsername.ForeColor = System.Drawing.Color.FromArgb(93, 242, 214);
            txtUsername.Location = new System.Drawing.Point(20, 90);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new System.Drawing.Size(320, 24);
            txtUsername.TabIndex = 2;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.ForeColor = System.Drawing.Color.FromArgb(141, 164, 164);
            lblUsername.Location = new System.Drawing.Point(20, 70);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new System.Drawing.Size(164, 17);
            lblUsername.TabIndex = 1;
            lblUsername.Text = "Tên đăng nhập (Username):";
            // 
            // lblDetailTitle
            // 
            lblDetailTitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblDetailTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            lblDetailTitle.ForeColor = System.Drawing.Color.FromArgb(93, 242, 214);
            lblDetailTitle.Location = new System.Drawing.Point(20, 20);
            lblDetailTitle.Name = "lblDetailTitle";
            lblDetailTitle.Size = new System.Drawing.Size(320, 35);
            lblDetailTitle.TabIndex = 0;
            lblDetailTitle.Text = "THÔNG TIN TÀI KHOẢN";
            // 
            // dgvUsers
            // 
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.BackgroundColor = System.Drawing.Color.FromArgb(8, 12, 13);
            dgvUsers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvUsers.ColumnHeadersHeight = 36;
            dgvUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvUsers.GridColor = System.Drawing.Color.FromArgb(30, 69, 62);
            dgvUsers.Location = new System.Drawing.Point(0, 60);
            dgvUsers.MultiSelect = false;
            dgvUsers.Name = "dgvUsers";
            dgvUsers.ReadOnly = true;
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.RowTemplate.Height = 32;
            dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.Size = new System.Drawing.Size(790, 640);
            dgvUsers.TabIndex = 2;
            dgvUsers.CellClick += dgvUsers_CellClick;
            // 
            // UserManagementForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(8, 12, 13);
            ClientSize = new System.Drawing.Size(1150, 700);
            Controls.Add(dgvUsers);
            Controls.Add(pnlDetails);
            Controls.Add(pnlHeader);
            Font = new System.Drawing.Font("Segoe UI", 9.5F);
            ForeColor = System.Drawing.Color.White;
            MinimumSize = new System.Drawing.Size(1000, 650);
            Name = "UserManagementForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "DTBStore - Quản Lý Tài Khoản Người Dùng";
            Load += UserManagementForm_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlDetails.ResumeLayout(false);
            pnlDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.Label lblDetailTitle;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.ComboBox cboRole;
        private System.Windows.Forms.Label lblPassTitle;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.Button btnResetPass;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAddNew;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnViewOrders;
        private System.Windows.Forms.DataGridView dgvUsers;
    }
}