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
    public partial class frmkhachhangthem : Form
    {
        private SqlConnection con;
        private DataTable dtCustomer = new DataTable("tblCustomer");
        private SqlDataAdapter da = new SqlDataAdapter();

        public frmkhachhangthem()
        {
            InitializeComponent();
        }
        private void Connect()
        {
            String cn = "Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=qms;Integrated Security=True";
            try {
                con = new SqlConnection(cn);
                con.Open();
            }
            catch (Exception e){
                MessageBox.Show("Khong the ket noi den du lieu ", "Error", MessageBoxButtons.OK);
                Console.WriteLine("\n Message Connection----\n", e.Message);
            }
        }
        private void Disconnect()
        {
            con.Close();
            con.Dispose();
            con = null;
        }

        private void GetData(){
            SqlCommand command = new SqlCommand();
            command.Connection = con;
            command.CommandType = CommandType.Text;
            command.CommandText = "Select * from Counter_Customer";
            da.SelectCommand = command;
            da.Fill(dtCustomer);

        }

        private void frmkhachhangthem_Load(object sender, EventArgs e)
        {
            Connect();
            GetData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
            Disconnect();
            Admin _Form1 = new Admin();
            _Form1.Show();

        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            DataRow row = dtCustomer.NewRow();
            //Lay du lieu
            row["IdCustomer"] = txtAddSoPhieu.Text;
            row["SCounter"] = txtAddQuay.Text;
            row["CStatus"] = "False";
            dtCustomer.Rows.Add(row);
            //Insert data
            SqlCommand commandInsert = new SqlCommand();
            commandInsert.Connection = con;
            commandInsert.CommandType = CommandType.Text;
            commandInsert.CommandText = @"insert into dbo.Counter_Customer (SCounter, IdCustomer, CStatus) values (@SCounter, @IdCustomer, @CStatus)";

            //Chen so quay
            commandInsert.Parameters.Add("@SCounter", SqlDbType.NVarChar, 10, "SCounter");
            commandInsert.Parameters.Add("@IdCustomer", SqlDbType.Int, 10,  "IdCustomer");
            commandInsert.Parameters.Add("@CStatus", SqlDbType.Bit, 2, "CStatus");

            da.InsertCommand = commandInsert;
            da.Update(dtCustomer);
            MessageBox.Show("Ban da them thanh cong", "Thong bao", MessageBoxButtons.OK);
            Close();
            //Disconnect();

           // Dispose();
           // Admin _Form1 = new Admin();
           // _Form1.Show();

        }
    }
}
