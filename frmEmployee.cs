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
using System.Threading;
using System.Collections;

namespace QMS
{
    public partial class frmEmployee : Form
    {
        private SqlConnection con;
        private DataTable dtEmployee = new DataTable("tblEmployee");
        private SqlDataAdapter daEmployee = new SqlDataAdapter();

        public frmEmployee()
        {
            InitializeComponent();
        }

        private void frmEmployee_Load(object sender, EventArgs e)
        {
            Connect();
            GetEmployeeInfor();
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

        #region Lay du lieu nhan vien giao dich
        private void GetEmployeeInfor()
        {
            SqlCommand command = new SqlCommand();
            command.Connection = con;
            command.CommandType = CommandType.Text;
            command.CommandText = "select * from dbo.Employee";
            daEmployee.SelectCommand = command;
            daEmployee.Fill(dtEmployee);
            dataGridView1.DataSource = dtEmployee;
            Binding();
        }
        #endregion

        private void Binding() {
            txtIdEmployee.DataBindings.Clear();
            txtIdEmployee.DataBindings.Add("Text", dataGridView1.DataSource, "IdEmployees");

            txtAccountEmployee.DataBindings.Clear();
            txtAccountEmployee.DataBindings.Add("Text", dataGridView1.DataSource, "Account");

            txtPassEmployee.DataBindings.Clear();
            txtPassEmployee.DataBindings.Add("Text", dataGridView1.DataSource, "Password");

            txtNameEmployee.DataBindings.Clear();
            txtNameEmployee.DataBindings.Add("Text", dataGridView1.DataSource, "NameEmployees");

            cbSexEmployee.DataBindings.Clear();
            cbSexEmployee.DataBindings.Add("Text", dataGridView1.DataSource, "Sex");

            txtPhoneEmployee.DataBindings.Clear();
            txtPhoneEmployee.DataBindings.Add("Text", dataGridView1.DataSource, "Phone");

            txtEmailEmployee.DataBindings.Clear();
            txtEmailEmployee.DataBindings.Add("Text", dataGridView1.DataSource, "Email");

            cbAuthorEmployee.DataBindings.Clear();
            cbAuthorEmployee.DataBindings.Add("Text", dataGridView1.DataSource, "Authority");

        }

        #region Them nhan vien
        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (txtIdEmployee.Text == "") {
                MessageBox.Show("Bạn chưa nhập mã nhân viên !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIdEmployee.Focus();
            }
            else if (txtAccountEmployee.Text == "") {
                MessageBox.Show("Bạn chưa nhập tài khoản nhân viên !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAccountEmployee.Focus();
            }
            else if (txtPassEmployee.Text == "") {
                MessageBox.Show("Bạn chưa nhập mật khẩu nhân viên !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassEmployee.Focus();
            }
            else if (txtNameEmployee.Text == "") {
                MessageBox.Show("Bạn chưa nhập tên nhân viên !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNameEmployee.Focus();
            }
            else if (cbAuthorEmployee.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập quyền truy cập nhân viên !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbAuthorEmployee.Focus();
            }
            else {

                if (FindIEmployees() == true) {
                    MessageBox.Show("Đã tồn tại mã nhân viên ! ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtIdEmployee.Focus();
                }
                else {

                    #region Chen du lieu vao csdl
                    DataRow row = dtEmployee.NewRow();
                    row["IdEmployees"] = txtIdEmployee.Text;
                    row["Account"] = txtAccountEmployee.Text;
                    row["Password"] = txtPassEmployee.Text;
                    row["NameEmployees"] = txtNameEmployee.Text;
                    row["Sex"] = cbSexEmployee.Text;
                    row["Phone"] = txtPhoneEmployee.Text;
                    row["Email"] = txtEmailEmployee.Text;
                    row["Authority"] = cbAuthorEmployee.Text;
                    dtEmployee.Rows.Add(row);

                    SqlCommand commandInsert = new SqlCommand();
                    commandInsert.Connection = con;
                    commandInsert.CommandType = CommandType.Text;
                    commandInsert.CommandText = @"insert into dbo.Employee (IdEmployees, Account, Password, NameEmployees, Sex, Phone, Email, Authority) 
                                        values (@IdEmployees, @Account, @Password, @NameEmployees, @Sex, @Phone, @Email, @Authority) ";
                    commandInsert.Parameters.Add("@IdEmployees", SqlDbType.NVarChar, 10, "IdEmployees");
                    commandInsert.Parameters.Add("@Account", SqlDbType.NVarChar, 50, "Account");
                    commandInsert.Parameters.Add("@Password", SqlDbType.NVarChar, 20, "Password");
                    commandInsert.Parameters.Add("@NameEmployees", SqlDbType.NVarChar, 50, "NameEmployees");
                    commandInsert.Parameters.Add("@Sex", SqlDbType.NVarChar, 10, "Sex");
                    commandInsert.Parameters.Add("@Phone", SqlDbType.NVarChar, 20, "Phone");
                    commandInsert.Parameters.Add("@Email", SqlDbType.NVarChar, 20, "Email");
                    commandInsert.Parameters.Add("@Authority", SqlDbType.NVarChar, 10, "Authority");

                    daEmployee.InsertCommand = commandInsert;
                    daEmployee.Update(dtEmployee);

                    commandInsert.Dispose();
                    daEmployee.Dispose();

                    #endregion               
                }

            }
        }
        #endregion

        #region Tim ma nhan vien trong csdl
        private bool FindIEmployees() {
            SqlDataAdapter daIdFind = new SqlDataAdapter();
            DataTable dtIdFind = new DataTable("tblIdFind");

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"select Employee.IdEmployees as N'IdEmployees' from dbo.Employee 
                                where IdEmployees LIKE '%' + @IdEmployees + '%' ";

            cmd.Parameters.Add("@IdEmployees", SqlDbType.NVarChar, 10).Value = txtIdEmployee.Text;

            daIdFind.SelectCommand = cmd;
            daIdFind.Fill(dtIdFind);
            if (dtIdFind.Rows.Count > 0) {
                cmd.Dispose();
                return true;
            }
            else
            {
                cmd.Dispose();
                return false;
            }

        }
        #endregion
    }
}
