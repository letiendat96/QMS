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
    public partial class frmDoiMatKhau : Form
    {
        private SqlConnection con;
        private DataTable dtUserPass = new DataTable("dtUserPass");
        private SqlDataAdapter daUserPass = new SqlDataAdapter();

        public frmDoiMatKhau()
        {
            InitializeComponent();
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

        private void GetUserData()
        {
            SqlCommand comand = new SqlCommand();
            comand.Connection = con;
            comand.CommandType = CommandType.Text;
            comand.CommandText = "select * from dbo.Employee";
            daUserPass.SelectCommand = comand;
            daUserPass.Fill(dtUserPass);
        }

        private void frmDoiMatKhau_Load(object sender, EventArgs e)
        {
            Connect();
            GetUserData();
        }

        #region  Ngat ket noi voi Server
        private void Disconnect()
        {
            con.Close();
            con.Dispose();
            con = null;
        }

        #endregion

        private void btnPassOK_Click(object sender, EventArgs e)
        {
            if (txtMKcu.Text == "") {
                MessageBox.Show("Bạn chưa nhập mật khẩu !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMKcu.Focus();
            }
            else if (txtMKmoi.Text == "" || txtMKcu.Text != txtConfimMk.Text)
            {
                MessageBox.Show("Mật khẩu không đúng !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMKmoi.Focus();
            }
            else { 
            
            }
          
        }

    }
}
