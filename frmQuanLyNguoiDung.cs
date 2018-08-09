using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO.Ports;


namespace QMS
{
    public partial class frmQuanLyNguoiDung : Form
    {
        private SqlConnection con;
        private DataTable dtUser = new DataTable("dtUser");
        private SqlDataAdapter daUser = new SqlDataAdapter();

        public frmQuanLyNguoiDung()
        {
            InitializeComponent();
        }
        private void frmQuanLyNguoiDung_Load(object sender, EventArgs e)
        {
            Connect();
            GetUserData();
        }

        #region Ket noi database
        private void Connect()
        {

            String cn = "Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=qms;Integrated Security=True";
            try
            {
                con = new SqlConnection(cn);
                con.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show("Khong the ket noi den du lieu ", "Error", MessageBoxButtons.OK);
                Console.WriteLine("\n Message Connection----\n", e.Message);
            }

        }
        #endregion

        #region  Ngat ket noi voi Server
        private void Disconnect()
        {
            con.Close();
            con.Dispose();
            con = null;
        }

        #endregion

        private void GetUserData(){
            SqlCommand comand = new SqlCommand();
            comand.Connection = con;
            comand.CommandType = CommandType.Text;
            comand.CommandText = "select * from dbo.Employee";
            daUser.SelectCommand = comand;
            daUser.Fill(dtUser);
            dataGridView1.DataSource = dtUser;
            Binding();

        }

        private void Binding() {
            txtHoten.DataBindings.Clear();
            txtHoten.DataBindings.Add("Text", dataGridView1.DataSource, "NameEmployees");

            txtTaikhoan.DataBindings.Clear();
            txtTaikhoan.DataBindings.Add("Text", dataGridView1.DataSource, "Account");

            txtMatkhau.DataBindings.Clear();
            txtMatkhau.DataBindings.Add("Text", dataGridView1.DataSource, "Password");

            txtConfirmMk.DataBindings.Clear();
            txtConfirmMk.DataBindings.Add("Text", dataGridView1.DataSource, "Password");

            cboGioiTinh.DataBindings.Clear();
            cboGioiTinh.DataBindings.Add("Text", dataGridView1.DataSource, "Sex");

            txtPhone.DataBindings.Clear();
            txtPhone.DataBindings.Add("Text", dataGridView1.DataSource, "Phone");

            txtEmail.DataBindings.Clear();
            txtEmail.DataBindings.Add("Text", dataGridView1.DataSource, "Email");

            cboAuthority.DataBindings.Clear();
            cboAuthority.DataBindings.Add("Text", dataGridView1.DataSource, "Authority");

        }

        private int RandomIdEmployees(int min , int max) {
            Random rd = new Random();
            return rd.Next(min, max);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            
            // Lay du lieu
            if (txtTaikhoan.Text == "") {
                MessageBox.Show("Bạn chưa nhập tài khoản !", "Thông báo", MessageBoxButtons.OK , MessageBoxIcon.Warning);
                txtTaikhoan.Focus();
            }
            else if (txtMatkhau.Text == "") {
                MessageBox.Show("Bạn chưa nhập mật khẩu !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatkhau.Focus();
            }
            else if (txtConfirmMk.Text == "" || txtConfirmMk.Text != txtMatkhau.Text) {
                MessageBox.Show("Mật khẩu không đúng !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmMk.Focus();
            }
            else {
                try {
                    DataRow row = dtUser.NewRow();
                    row["IdEmployees"] = Convert.ToString(RandomIdEmployees(20180000, 20189999));
                    row["Account"] = txtTaikhoan.Text;
                    row["Password"] = txtConfirmMk.Text;
                    row["NameEmployees"] = txtHoten.Text;
                    row["Sex"] = cboGioiTinh.Text;
                    row["Phone"] = txtPhone.Text;
                    row["Email"] = txtEmail.Text;
                    row["Authority"] = cboAuthority.Text;

                    dtUser.Rows.Add(row);
                    // Insert data 
                    SqlCommand commandInsert = new SqlCommand();
                    commandInsert.Connection = con;
                    commandInsert.CommandType = CommandType.Text;
                    commandInsert.CommandText = "insert into dbo.Employee (IdEmployees, Account, Password, NameEmployees, Sex, Phone, Email, Authority) values( @IdEmployees,@Account, @Password, @NameEmployees, @Sex, @Phone, @Email, @Authority )";

                    commandInsert.Parameters.Add("@IdEmployees", SqlDbType.Int, 10, "IdEmployees");
                    commandInsert.Parameters.Add("@Account", SqlDbType.NVarChar, 50, "Account");
                    commandInsert.Parameters.Add("@Password", SqlDbType.NVarChar, 20, "Password");
                    commandInsert.Parameters.Add("@NameEmployees", SqlDbType.NVarChar, 50, "NameEmployees");
                    commandInsert.Parameters.Add("@Sex", SqlDbType.NVarChar, 10, "Sex");
                    commandInsert.Parameters.Add("@Phone", SqlDbType.NVarChar, 20, "Phone");
                    commandInsert.Parameters.Add("@Email", SqlDbType.NVarChar, 20, "Email");
                    commandInsert.Parameters.Add("@Authority", SqlDbType.NVarChar, 20, "Authority");

                    daUser.InsertCommand = commandInsert;
                    daUser.Update(dtUser);
                
                }catch(Exception ex){
                    MessageBox.Show(ex.ToString(), "Thông báo", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                
                }               

            }
             
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Lay du lieu
            if (txtTaikhoan.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập tài khoản !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTaikhoan.Focus();
            }
            else if (txtMatkhau.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập mật khẩu !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatkhau.Focus();
            }
            else if (txtConfirmMk.Text == "" || txtConfirmMk.Text != txtMatkhau.Text)
            {
                MessageBox.Show("Mật khẩu không đúng !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmMk.Focus();
            }
            else {
                DataRow row = dtUser.Select("Account = " + "'" +txtTaikhoan.Text +"'")[0];
                row.BeginEdit();
                row["Password"] = txtConfirmMk.Text;
                row["NameEmployees"] = txtHoten.Text;
                row["Sex"] = cboGioiTinh.Text;
                row["Phone"] = txtPhone.Text;
                row["Email"] = txtEmail.Text;
                row["Authority"] = cboAuthority.Text;
                row.EndEdit();

                SqlCommand commandUpdate = new SqlCommand();
                commandUpdate.Connection = con;
                commandUpdate.CommandType = CommandType.Text;
                commandUpdate.CommandText = @"Update dbo.Employee set Password = @Password, NameEmployees = @NameEmployees, Sex = @Sex, Phone = @Phone, Email = @Email, Authority = @Authority where Account = @Account";

                //commandUpdate.Parameters.Add("@IdEmployees", SqlDbType.Int, 10, "IdEmployees");
                commandUpdate.Parameters.Add("@Account", SqlDbType.NVarChar, 50, "Account");
                commandUpdate.Parameters.Add("@Password", SqlDbType.NVarChar, 20, "Password");
                commandUpdate.Parameters.Add("@NameEmployees", SqlDbType.NVarChar, 50, "NameEmployees");
                commandUpdate.Parameters.Add("@Sex", SqlDbType.NVarChar, 10, "Sex");
                commandUpdate.Parameters.Add("@Phone", SqlDbType.NVarChar, 20, "Phone");
                commandUpdate.Parameters.Add("@Email", SqlDbType.NVarChar, 20, "Email");
                commandUpdate.Parameters.Add("@Authority", SqlDbType.NVarChar, 20, "Authority");

                daUser.UpdateCommand = commandUpdate;
                daUser.Update(dtUser);
                MessageBox.Show("Bạn đã sửa thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtUser.Clear();

                GetUserData();            
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataRow row = dtUser.Select("Account = " + "'" + txtTaikhoan.Text + "'")[0];
            row.BeginEdit();
            row.Delete();
            row.EndEdit();

            SqlCommand commandDelete = new SqlCommand();
            commandDelete.Connection = con;
            commandDelete.CommandType = CommandType.Text;
            commandDelete.CommandText = @"Delete from dbo.Employee where Account = @Account";
            commandDelete.Parameters.Add("@Account", SqlDbType.NVarChar, 50, "Account");
            //-------------------------------------------------------------------------------
            daUser.DeleteCommand = commandDelete;
            daUser.Update(dtUser);
            dtUser.Clear();
            GetUserData();

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
