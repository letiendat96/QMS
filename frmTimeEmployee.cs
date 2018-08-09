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
    public partial class frmTimeEmployee : Form
    {
        private SqlConnection con;
        private DataTable dtTimeEmployee = new DataTable("tblTimeEmployee");
        private SqlDataAdapter daTimeEmployee = new SqlDataAdapter();

        public frmTimeEmployee()
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

        #region  Ngat ket noi voi Server
        private void Disconnect()
        {
            con.Close();
            con.Dispose();
            con = null;
        }

        #endregion

        #region Lay du lieu nhan vien giao dich
        private void GetTimeEmployee()
        {
            SqlCommand command = new SqlCommand();
            command.Connection = con;
            command.CommandType = CommandType.Text;
            command.CommandText = @"select Id, NameEmployees ,SCounter, EmployeeStatus,
                                    TimeStart,TimeEnd, CustomerServed, CustomerServeing 
                                    from dbo.Employee_Counter, dbo.Counter, dbo.Employee
                                    where (Counter.IdCounter = Employee_Counter.SCounter)
                                    and (Employee.IdEmployees = Employee_Counter.Id)";
            daTimeEmployee.SelectCommand = command;
            daTimeEmployee.Fill(dtTimeEmployee);
            dataGridView1.DataSource = dtTimeEmployee;
            
        }
        #endregion

        private void frmTimeEmployee_Load(object sender, EventArgs e)
        {
            Connect();
            GetTimeEmployee();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dtTimeEmployee.Clear();
            GetTimeEmployee();
        }    
    }
}
