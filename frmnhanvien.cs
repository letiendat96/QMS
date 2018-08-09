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


namespace QMS
{
    public partial class frmnhanvien : Form
    {
        private SqlConnection con;
        private DataTable dtEmployees = new DataTable("tblEmployee");
        private SqlDataAdapter da = new SqlDataAdapter();

        private void Connect() {
            String cn = "Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=qms;Integrated Security=True";
            try{
                con = new SqlConnection(cn);
                con.Open();
            }
            catch (Exception e) {
                MessageBox.Show("Khong the ket noi den du lieu ", "Error", MessageBoxButtons.OK);
                Console.WriteLine("\n Message Connection----\n", e.Message);
            }
        }

        private void Disconnect(){
            con.Close();
            con.Dispose();
            con = null;
        }

        public frmnhanvien() {
            InitializeComponent();
        }

        private void GetData()
        {
            SqlCommand command = new SqlCommand(); // Khai bao 1 command
            command.Connection = con; // Ket noi
            command.CommandType = CommandType.Text; // Khai bao kieu command
            command.CommandText = @"select * from dbo.Employees";
            da.SelectCommand = command;
            da.Fill(dtEmployees);
            dataGridView2.DataSource = dtEmployees;
            SetBinding();
            
        }

        private void SetBinding() {
            txtnhanvienId.DataBindings.Clear();
            txtnhanvienId.DataBindings.Add("Text", dataGridView2.DataSource, "IdEmployees");
            txtnhanvienTen.DataBindings.Clear();
            txtnhanvienTen.DataBindings.Add("Text", dataGridView2.DataSource, "SCounter");

        }
        private void frmnhanvien_Load(object sender, EventArgs e)
        {
            Connect();
            GetData();
            Disconnect();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        
    }
}
