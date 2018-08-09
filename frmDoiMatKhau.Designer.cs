namespace QMS
{
    partial class frmDoiMatKhau
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDoiMatKhau));
            this.btnExitPass = new System.Windows.Forms.Button();
            this.btnPassOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMKcu = new System.Windows.Forms.TextBox();
            this.txtConfimMk = new System.Windows.Forms.TextBox();
            this.txtMKmoi = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExitPass
            // 
            this.btnExitPass.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExitPass.ForeColor = System.Drawing.Color.Black;
            this.btnExitPass.Image = ((System.Drawing.Image)(resources.GetObject("btnExitPass.Image")));
            this.btnExitPass.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExitPass.Location = new System.Drawing.Point(337, 298);
            this.btnExitPass.Name = "btnExitPass";
            this.btnExitPass.Size = new System.Drawing.Size(92, 39);
            this.btnExitPass.TabIndex = 20;
            this.btnExitPass.Text = "Thoát ";
            this.btnExitPass.UseVisualStyleBackColor = true;
            // 
            // btnPassOK
            // 
            this.btnPassOK.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPassOK.ForeColor = System.Drawing.Color.Black;
            this.btnPassOK.Image = ((System.Drawing.Image)(resources.GetObject("btnPassOK.Image")));
            this.btnPassOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPassOK.Location = new System.Drawing.Point(179, 298);
            this.btnPassOK.Name = "btnPassOK";
            this.btnPassOK.Size = new System.Drawing.Size(92, 39);
            this.btnPassOK.TabIndex = 19;
            this.btnPassOK.Text = "Ok";
            this.btnPassOK.UseVisualStyleBackColor = true;
            this.btnPassOK.Click += new System.EventHandler(this.btnPassOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtMKcu);
            this.groupBox1.Controls.Add(this.txtConfimMk);
            this.groupBox1.Controls.Add(this.txtMKmoi);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(23, 94);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(558, 173);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin";
            // 
            // txtMKcu
            // 
            this.txtMKcu.Location = new System.Drawing.Point(237, 32);
            this.txtMKcu.Name = "txtMKcu";
            this.txtMKcu.Size = new System.Drawing.Size(182, 20);
            this.txtMKcu.TabIndex = 16;
            // 
            // txtConfimMk
            // 
            this.txtConfimMk.Location = new System.Drawing.Point(237, 111);
            this.txtConfimMk.Name = "txtConfimMk";
            this.txtConfimMk.Size = new System.Drawing.Size(182, 20);
            this.txtConfimMk.TabIndex = 15;
            // 
            // txtMKmoi
            // 
            this.txtMKmoi.Location = new System.Drawing.Point(237, 72);
            this.txtMKmoi.Name = "txtMKmoi";
            this.txtMKmoi.Size = new System.Drawing.Size(182, 20);
            this.txtMKmoi.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(115, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Đánh lại mật khẩu";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(115, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Mật khẩu mới";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(115, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Mật khẩu cũ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(156, 44);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(273, 26);
            this.label8.TabIndex = 17;
            this.label8.Text = "CẬP NHẬT MẬT KHẨU";
            // 
            // frmDoiMatKhau
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 396);
            this.Controls.Add(this.btnExitPass);
            this.Controls.Add(this.btnPassOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label8);
            this.Name = "frmDoiMatKhau";
            this.Text = "frmDoiMatKhau";
            this.Load += new System.EventHandler(this.frmDoiMatKhau_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExitPass;
        private System.Windows.Forms.Button btnPassOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtMKcu;
        private System.Windows.Forms.TextBox txtConfimMk;
        private System.Windows.Forms.TextBox txtMKmoi;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
    }
}