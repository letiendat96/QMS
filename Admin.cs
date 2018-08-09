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
// Kiem tra lai queue
namespace QMS
{
    public partial class Admin : Form
    {
        private SqlConnection con;
        private DataTable dtCustomer = new DataTable("tblCustomer");
        private DataTable dtCounter = new DataTable("tblCounter");
        private DataTable dtEmployeeLogin = new DataTable("tblEmployeeLogin");
        private DataTable dtCustomer2 = new DataTable("tblCustomer2");
        private DataTable dtCustomer3 = new DataTable("tblCustomer3");
        private DataTable dtCusServed2 = new DataTable("tblCusServed2");
        private DataTable dtCusServed3 = new DataTable("tblCusServed3");

        private DataTable dtCusServeing2 = new DataTable("tblCusServeing2");
        private DataTable dtCusServeing3 = new DataTable("tblCusServeing3");


        private SqlDataAdapter da = new SqlDataAdapter();
        private SqlDataAdapter dCounter = new SqlDataAdapter();
        private SqlDataAdapter dtEmployee = new SqlDataAdapter();
        private SqlDataAdapter dCustomer2 = new SqlDataAdapter();
        private SqlDataAdapter dCustomer3 = new SqlDataAdapter();
        private SqlDataAdapter dCusServed2 = new SqlDataAdapter();
        private SqlDataAdapter dCusServed3 = new SqlDataAdapter();
        private SqlDataAdapter dCusServeing2 = new SqlDataAdapter();
        private SqlDataAdapter dCusServeing3 = new SqlDataAdapter();


        private Boolean openPort = true;
        public char data;
        private bool STC1 = false, STC2 = false, STC3 = false, STC4 = false;
        private string Counter;
        private int Customer = 0;
        //------------------------------------------------------------------------
        private byte[] buffer = new byte[1024];
        private string s = "abc";
        private Queue queue2 = new Queue();
        private Queue queue3 = new Queue();
        //-------------------------------------------------------------------------
        string quay2 = "e21";
        string quay3 = "e31";       
        bool ack1 = false; bool ack2 = false; bool ack3 = false; bool ack4 = false;

        private string[] ArrayString = new string[1024];
        //------------------------------------------------------------------------
        private string ackCustomer;
        private string _ackCustomer;
        //------------------------------------------------------------------------
        private int index2 = 1;
        private int index3 = 1;
        //------------------------------------------------------------------------
        private char[] mSerial = new char[8];
        private char[] xSerial = new char[8];
        private char[] mAck = new char[7];

        private bool sThread = true;
        private string statusEmployees = "True";
        private string time;     
        private string servedCus2, servedCus3, serveingCus2, serveingCus3;
        //-----------------------------------------------------------------------
        private int[] ArrayCustomer2 = new int[10000];
        private int arrIndex2 = 0;

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

        public Admin()
        {
            InitializeComponent();
            
        }

        #region Ket noi sql, mo cong Port
        private void Form1_Load(object sender, EventArgs e)
        {
            Connect();
            GetCounterCustomer();
            GetCounter();
            GetEmployeeLogin();

            GetCustomer2();
            GetCustomerServed2();
            GetCustomerServeing2();

            GetCustomer3();
            GetCustomerServed3();
            GetCustomerServeing3();

            string[] Port = SerialPort.GetPortNames();
            cbPortBox.Items.AddRange(Port);
            cbPortBox.SelectedIndex = 0;
            chkOld.Checked = true;
            chkUpdate.Checked = false;
            timer1.Start();

        }
        #endregion

        #region Lay du quay - khach hang tu Server
        private void GetCounterCustomer()
        {
            SqlCommand command = new SqlCommand(); // Khai bao 1 command
            command.Connection = con; // Ket noi
            command.CommandType = CommandType.Text; // Khai bao kieu command
            command.CommandText = @"select * from dbo.Counter_Customer";
            da.SelectCommand = command;                 
            da.Fill(dtCustomer);
            dataGridView1.DataSource = dtCustomer;
            //Bindind();
        }
        #endregion
        // Tim kiem trong bang id nhan vien 
        private void GetEmployeeLogin() {
            SqlCommand command = new SqlCommand();
            command.Connection = con;
            command.CommandType = CommandType.Text;
            command.CommandText = "select * from dbo.Employee_Counter";
            dtEmployee.SelectCommand = command;
            dtEmployee.Fill(dtEmployeeLogin);

        }

        #region Lay du lieu quay tu Server
        private void GetCounter()
        {
            SqlCommand command = new SqlCommand(); // Khai bao 1 command
            command.Connection = con; // Ket noi
            command.CommandType = CommandType.Text; // Khai bao kieu command
            command.CommandText = @"select * from dbo.Counter";
            dCounter.SelectCommand = command;
            dCounter.Fill(dtCounter);
        }

        #endregion

        //#region Binding Data khach hang, quay
        //private void Bindind()
        //{
        //    txtSoPhieu.DataBindings.Clear();
        //    txtSoPhieu.DataBindings.Add("Text", dataGridView1.DataSource, "IdCustomer");
        //    txtQuay.DataBindings.Clear();
        //    txtQuay.DataBindings.Add("Text", dataGridView1.DataSource, "SCounter");
        //}
        //#endregion
         

        private void button6_Click(object sender, EventArgs e)
        {
            if ((MessageBox.Show("Ban co chac chan muon thoat khong ?", "Thoat", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                this.Close();
                System.Windows.Forms.Application.Exit();
            }
        }


        //#region Chinh sua thong tin khach hang, so quay
        //private void button2_Click(object sender, EventArgs e)
        //{
        //    if (txtSoPhieu.Text.Length > 0)
        //    {
        //        DataRow row = dtCustomer.Select("IdCustomer = " + Convert.ToInt32(txtSoPhieu.Text))[0];
        //        row.BeginEdit();
        //        row["SCounter"] = txtQuay.Text;
        //        row.EndEdit();

        //        SqlCommand commandUpdate = new SqlCommand();
        //        commandUpdate.Connection = con;
        //        commandUpdate.CommandType = CommandType.Text;
        //        commandUpdate.CommandText = @"Update dbo.Counter_Customer set SCounter = @SCounter where IdCustomer = @IdCustomer";

        //        commandUpdate.Parameters.Add("@IdCustomer", SqlDbType.Int, 10, "IdCustomer");
        //        commandUpdate.Parameters.Add("@SCounter", SqlDbType.NVarChar, 10, "SCounter");

        //        da.UpdateCommand = commandUpdate;
        //        da.Update(dtCustomer);
        //        MessageBox.Show("Ban da sua thanh cong", "Thong bao", MessageBoxButtons.OK);
        //        dtCustomer.Clear();
        //        GetCounterCustomer();

        //    }

        //}
        //#endregion

        //#region Xoa du lieu
        //private void button3_Click(object sender, EventArgs e)
        //{
        //    if (txtSoPhieu.Text.Length > 0)
        //    {
        //        DataRow row = dtCustomer.Select("IdCustomer = " + Convert.ToInt32(txtSoPhieu.Text))[0];
        //        row.BeginEdit();
        //        row.Delete();
        //        row.EndEdit();

        //        SqlCommand commandDelete = new SqlCommand();
        //        commandDelete.Connection = con;
        //        commandDelete.CommandType = CommandType.Text;
        //        commandDelete.CommandText = @"Delete from dbo.Counter_Customer where IdCustomer = @IdCustomer";

        //        commandDelete.Parameters.Add("@IdCustomer", SqlDbType.Int, 10, "IdCustomer");
        //        da.DeleteCommand = commandDelete;
        //        da.Update(dtCustomer);

        //        MessageBox.Show("Ban da xoa ", "Thong bao", MessageBoxButtons.OK);
        //        dtCustomer.Clear();
        //        GetCounterCustomer();

        //    }
        //    else
        //    {
        //        MessageBox.Show("Khong co du lieu ", "Thong bao", MessageBoxButtons.OK);
        //    }

        //}
        //#endregion

//        #region Tim kiem
//        private void btnSearch_Click(object sender, EventArgs e)
//        {
//            dtCustomer.Clear();
//            SqlCommand command = new SqlCommand();
//            command.Connection = con;
//            command.CommandType = CommandType.Text;
//            command.CommandText = @"select Counter_Customer.IdCustomer as N'IdCustomer', 
//                                    SCounter as N'SCounter' from dbo.Counter_Customer
//                                    where IdCustomer LIKE '%' + @IdCustomer + '%'";

//            command.Parameters.Add("@IdCustomer", SqlDbType.NVarChar, 50).Value = txtSearch.Text;
//            da.SelectCommand = command;
//            da.Fill(dtCustomer);
//            if (dtCustomer.Rows.Count > 0)
//            {
//                dataGridView1.DataSource = dtCustomer;
//            }
//            else
//            {
//                MessageBox.Show("Khong co du lieu", "Thong Bao", MessageBoxButtons.OK);
//            }


//        }
//        #endregion

        #region Open Port
        private void btnPower_Click(object sender, EventArgs e)
        {
            if (openPort == true)
            {
                try
                {
                    serialPort1.PortName = cbPortBox.Text;
                    serialPort1.Open();
                    serialPort1.BaudRate = 9600;
                    serialPort1.DataBits = 8;
                    serialPort1.Handshake = Handshake.None;
                    serialPort1.Parity = Parity.None;
                    serialPort1.StopBits = StopBits.One;

                    try
                    {
                        if (serialPort1.IsOpen)
                        {
                            txtReceivePort.Text = serialPort1.ReadExisting();

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Thong Bao", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                    btnPower.BackColor = Color.Blue;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Thong Bao", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

                openPort = false;
            }
            else
            {
                openPort = true;
                try
                {
                    serialPort1.Close();
                    btnPower.BackColor = Color.Red;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Thong Bao", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
        }
        #endregion

        #region Gui du lieu
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.WriteLine(txtSendPort.Text + Environment.NewLine);
                    txtSendPort.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thong Bao", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        //#region Nhan du lieu
        //private void btnReceive_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (serialPort1.IsOpen)
        //        {
        //            txtReceivePort.Text = serialPort1.ReadExisting();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Thong Bao", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        //#endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
        }

        private static bool CheckContain(string s1, string s2)
        {
            if (s1.Contains(s2))
            {
                return true;
            }
            return false;
        }

        #region Nhan du lieu lien tuc
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Doc du lieu ve 
            try {
                data = (char)serialPort1.ReadChar();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }

            this.Invoke(new EventHandler(ShowData));
           
            #region Nhan vien dang nhap 
            if (data == 'e')
            {
                for (int i = 0; i < 5; i++)
                {
                    mSerial[i] = (char)serialPort1.ReadChar();
                }
                if (mSerial[0] == '2')
                {
                    Thread.Sleep(500);
                    STC2 = true;
                    string s = new string(mSerial);                   
                    string sEmployees = s.Substring(1, 4);                  
                    if (SearchEmployee(sEmployees))
                    {                       
                        statusEmployees = "True";
                        StatusEmployeeLogin(sEmployees);

                        Thread.Sleep(1000);
                        SendSerial("se21");

                        Thread.Sleep(1000);
                        // Kiem tra sl kh dang phuc vu
                        if (serveingCus2.Equals("0"))
                        {
                            SendSerial("sa20000");
                        }
                        else {
                            // Dung hang doi de gui
                            string id2Customer = queue2.Dequeue().ToString();
                            id2Customer = "sa2" + id2Customer;
                            //SendSerial(id2Customer);    

                            // Dung mang de gui
                            string arrCustomer2 = ArrayCustomer2[arrIndex2].ToString();
                            string led32 = "h2" + arrCustomer2; 
                            arrCustomer2 = "sa2" + arrCustomer2;
                            
                            SendSerial(led32);
                            Thread.Sleep(1000);
                            SendSerial(arrCustomer2);

                        }
                        
                    }
                    else {
                        Thread.Sleep(1000);
                        SendSerial("se20");
                    }
                }
                else if (mSerial[0] == '3')
                {
                    Thread.Sleep(500);
                    STC3 = true;
                    string s = new string(mSerial);                   
                    string sEmployees = s.Substring(1, 4);
                    if (SearchEmployee(sEmployees))
                    {
                        statusEmployees = "True";
                        StatusEmployeeLogin(sEmployees);

                        Thread.Sleep(1000);
                        SendSerial("se31");

                        Thread.Sleep(1000);
                        if (serveingCus3.Equals("0"))
                        {
                            SendSerial("sa30000");
                        }
                        else {
                            // Dung hang doi de gui
                            string id3Customer = queue3.Dequeue().ToString();
                            string led33 = "h3" + id3Customer;
                            id3Customer = "sa3" + id3Customer;

                            SendSerial(led33);                          
                            Thread.Sleep(1000);
                            SendSerial(id3Customer);
                            
                        }
                        
                    }
                    else {
                        Thread.Sleep(1000);
                        SendSerial("se30");
                    }

                }
                else {
                    STC2 = STC3 = false;
                }

                #region Da luong
                //if (mSerial[0] == '2' && mSerial[1] == '1') {
                //    SendSerial("se21");
                //    STC2 = true;

                //    Thread.Sleep(500);
                //    SendSerial("sa26789");

                //    #region Vao luong xu ly
                //    //Thread process2 = new Thread(
                //    //    new ThreadStart(this.Process2)
                //    //    );

                //    //Thread mprocess2 = new Thread(
                //    //    new ThreadStart (this.Process2)
                //    //    );
                //    if (sThread)
                //    {
                //        sThread = false;
                //       // process2.Start();
                //       // mprocess2.Abort();
                //    }
                //    else {
                //        sThread = true;
                //       // process2.Abort();
                //       // mprocess2.Start();                  
                //    }
                //    #endregion

                //}
                //else if (mSerial[0] == '3' && mSerial[1] == '1')
                //{
                //    SendSerial("se31");
                //    STC3 = true;
                //    Thread.Sleep(500);
                //    SendSerial("sa31234");

                //    #region Vao luong xu ly
                //    //Thread process3 = new Thread(
                //    //    delegate() {
                //    //        if (STC3 == true && ack3 == false)
                //    //        {
                //    //            Thread.Sleep(5000);
                //    //            SendSerial("sa31234");
                //    //        }

                //    //    }
                //    //    );
                //    //process3.Start();
                //    #endregion

                //}
                //else {
                //    STC2 = STC3 = false;
                //}
                #endregion
            }
            #endregion
 
            #region Nhan vien dang xuat
            else if (data == 'd')
            {
                for (int i = 0; i < 5; i++) {
                    xSerial[i] = (char)serialPort1.ReadChar();
                }
                if (xSerial[0] == '2')
                {
                    Thread.Sleep(500);
                    string s = new string(xSerial);
                    string sEmployees = s.Substring(1, 4);
                    if (SearchEmployee(sEmployees))  {
                        statusEmployees = "False";

                        StatusEmployeeLogout(sEmployees);
                        Thread.Sleep(1000);
                        SendSerial("sd21");
                        StatusCounter("2", "False");
                        STC2 = false;                    

                    }

                }
                else if (xSerial[0] == '3') {
                    Thread.Sleep(500);
                    string s = new string(xSerial);
                    string sEmployees = s.Substring(1, 4);                    
                    if (SearchEmployee(sEmployees))
                    {
                        statusEmployees = "False";
                        StatusEmployeeLogout(sEmployees);
                        Thread.Sleep(1000);
                        SendSerial("sd31");
                        StatusCounter("3", "False");
                        STC3 = false;

                    }

                }
            }
            #endregion

            #region ACK , da phuc vu khach hang
            else if (data == 'b')
            {
                for (int i = 0; i < 6; i++)
                {
                    mAck[i] = (char)serialPort1.ReadChar();
                }
                if (mAck[0] == '2' && mAck[5] == '0')
                {
                    ack2 = true;
                }
                else if (mAck[0] == '2' && mAck[5] == '1')
                {
                    SendSerial("so2");
                    Thread.Sleep(500);

                    string serve2 = new string(mAck);
                    serve2 = serve2.Substring(1, 4);
                    //MessageBox.Show(serve2);
                    //Cap nhat vao csdl 
                    UpdateStatusCustomer(serve2);

                    // Dung hang doi de gui
                    string idCustomerNext2 = queue2.Dequeue().ToString();                    
                    idCustomerNext2 = "sa2" + idCustomerNext2;
                    // SendSerial(idCustomerNext2);

                    //SendSerial("sa21122");
                    //GetCustomer2();
                    // Dung mang de gui 
                    arrIndex2 = arrIndex2 + 1;
                    string arrCustomer2 = ArrayCustomer2[arrIndex2].ToString();
                    string arrCustomer2L = "h2" + arrCustomer2; 
                    arrCustomer2 = "sa2" + arrCustomer2;
                    SendSerial(arrCustomer2L);
                    Thread.Sleep(1000);
                    SendSerial(arrCustomer2);

                }
                else if (mAck[0] == '3' && mAck[5] == '0')
                {
                    ack3 = true;
                }
                else if (mAck[0] == '3' && mAck[5] == '1')
                {
                    SendSerial("so3");
                    Thread.Sleep(500);

                    string serve3 = new string(mAck);
                    serve3 = serve3.Substring(1, 4);
                    //MessageBox.Show(serve2);
                    UpdateStatusCustomer(serve3);

                    string idCustomerNext3 = queue3.Dequeue().ToString();
                    string arrCustomer3 = "h3" + idCustomerNext3;
                    idCustomerNext3 = "sa3" + idCustomerNext3;
                   
                    SendSerial(arrCustomer3);
                    Thread.Sleep(1000);
                    SendSerial(idCustomerNext3);

                    //SendSerial("sa36969");
                }
            }
            #endregion 
            Thread.Sleep(50);
            SetStatusCounter();
        }

        private void ResetAck(char [] A) {
            for (int i = 0; i < A.Length; i++) {
                A[i] = '0';
            }
        }

        #region Vao luong xu ly
        //private void Process2() {
        //    if (STC2 == true && ack2 == false)
        //    {
        //        Thread.Sleep(5000);
        //        SendSerial("sa26789");
        //    }
        
        //}

        //private void Process3()
        //{
        //    if (STC2 == true && ack2 == false)
        //    {
        //        Thread.Sleep(5000);
        //        SendSerial("sa26789");
        //    }

        //}
        #endregion

        #endregion

        //private void btnBuffer_Click(object sender, EventArgs e)
        //{
        //    txtBuffer.Text = "";
        //    foreach (string c in queue2)
        //    {
        //        txtBuffer.Text += c;
        //    }
        //    MessageBox.Show(s, "Buffer");
        //}

        #region Hien thi du lieu
        private void ShowData(object sender, EventArgs e)
        {        

            if (chkUpdate.Checked)
            {
                //string temp = new string(data);
                //txtReceivePort.Text = data;
            }
            else if (chkOld.Checked)
            {
                txtReceivePort.Text += "RX-" + data;
            }

        }
        #endregion
        // Kiem tra trang thai cac quay phuc vu
        public void CheckEmployeeLogin(string s)
        {
            switch (s)
            {
                case "e11":
                    STC1 = true;
                    SendSerial("e11");
                    break;
                case "e21":
                    STC2 = true;
                    try
                    {
                        SendSerial("e21");
                        Thread.Sleep(1000);
                        SendSerial("e21");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }

                    break;
                case "e31":
                    STC3 = true;
                    try
                    {
                        SendSerial("e31");
                        Thread.Sleep(1000);
                        SendSerial("e31");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }

                    break;
                case "e41":
                    STC4 = true;
                    SendSerial("e41");
                    break;
            }

        }

        private void StatusCounter(String Counter, String S)
        {
            DataRow row = dtCounter.Select("IdCounter = " + Convert.ToInt32(Counter))[0];
            row.BeginEdit();
            row["Status"] = S;
            row.EndEdit();

            SqlCommand commandUpdate = new SqlCommand();
            commandUpdate.Connection = con;
            commandUpdate.CommandType = CommandType.Text;
            commandUpdate.CommandText = @"Update dbo.Counter set Status = @Status where IdCounter = @IdCounter";

            commandUpdate.Parameters.Add("@IdCounter", SqlDbType.NVarChar, 10, "IdCounter");
            commandUpdate.Parameters.Add("@Status", SqlDbType.Bit, 10, "Status");

            dCounter.UpdateCommand = commandUpdate;
            dCounter.Update(dtCounter);
            //MessageBox.Show("Da cap nhat trang thai cac quay" + Counter , "Thong bao", MessageBoxButtons.OK);

        }
        //Chinh sua trang thai cac quay
        public void SetStatusCounter()
        {
            if (STC1){
                StatusCounter("1", "True");
            }
            else
            {
                StatusCounter("1", "False");
            }
            if (STC2)
            {
                StatusCounter("2", "True");
            }
            else
            {
                StatusCounter("2", "False");
            }
            if (STC3)
            {
                StatusCounter("3", "True");
            }
            else
            {
                StatusCounter("3", "False");
            }
            if (STC4)
            {
                StatusCounter("4", "True");
            }
            else
            {
                StatusCounter("4", "False");
            }

        }

        // Gui id khach - id quay => led 3 hang, led nhan vien, ban phim nhan vien 
        // Doc bang Couter_Customer

        public void SendDataAll()
        {
            foreach (DataRow dr in dtCustomer.Rows)
            {
                Customer = Convert.ToInt16(dr["IdCustomer"].ToString());
                Counter = dr["SCounter"].ToString();

                if (Customer < 10)
                {
                    string msg1 = "a" + Counter + "000" + Customer.ToString() + "t";
                    string msg2 = "c" + Counter + "000" + Customer.ToString() + "t";
                    string msg4 = "e" + Counter + "000" + Customer.ToString() + "t";
                    SendSerial(msg1); SendSerial(msg2); SendSerial(msg4);

                }
                else if (Customer >= 10 && Customer < 100)
                {
                    string msg1 = "a" + Counter + "00" + Customer.ToString() + "t";
                    string msg2 = "c" + Counter + "00" + Customer.ToString() + "t";
                    string msg4 = "e" + Counter + "00" + Customer.ToString() + "t";
                    SendSerial(msg1); SendSerial(msg2); SendSerial(msg4);
                }
                else if (Customer >= 100 && Customer < 1000)
                {
                    string msg1 = "a" + Counter + "0" + Customer.ToString() + "t";
                    string msg2 = "c" + Counter + "0" + Customer.ToString() + "t";
                    string msg4 = "e" + Counter + "0" + Customer.ToString() + "t";
                    SendSerial(msg1); SendSerial(msg2); SendSerial(msg4);
                }
                else if (Customer >= 1000 && Customer < 10000)
                {
                    string msg1 = "a" + Counter + Customer.ToString() + "t";
                    string msg2 = "c" + Counter + Customer.ToString() + "t";
                    string msg4 = "e" + Counter + Customer.ToString() + "t";
                    SendSerial(msg1); SendSerial(msg2); SendSerial(msg4);
                }

            }


        }

        #region Gui du lieu kieu string xuong serial
        public  void SendSerial(String s)
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.WriteLine(s);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Thay doi trang thai check
        private void chkUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUpdate.Checked)
            {
                chkUpdate.Checked = true;
                chkOld.Checked = false;
            }

        }

        private void chkOld_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOld.Checked)
            {
                chkUpdate.Checked = false;
                chkOld.Checked = true;
            }
        }
        #endregion

        private void quảnLýNgườiDùngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmQuanLyNguoiDung _frm = new frmQuanLyNguoiDung();
            _frm.Show();

        }

        private void đổiMậtKhẩuToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnAckCustomer_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ackCustomer, "ackCustomer");
        }

        private void txtReceivePort_TextChanged(object sender, EventArgs e)
        {

        }

        private bool SearchEmployee(string s) {
            dtEmployeeLogin.Clear();
            SqlCommand command = new SqlCommand();
            command.Connection = con;
            command.CommandType = CommandType.Text;
            command.CommandText = @"select Employee_Counter.Id as N'Id' , SCounter as N'SCounter' from dbo.Employee_Counter where Id LIKE '%' + @Id + '%'";
            command.Parameters.Add("@Id", SqlDbType.NVarChar, 50).Value = s;
            dtEmployee.SelectCommand = command;
            dtEmployee.Fill(dtEmployeeLogin);
            if (dtEmployeeLogin.Rows.Count > 0) {
                return true;               
            }
            else{
                return false;
            }
        }

        private void StatusEmployeeLogin(string s) {
            DataRow row = dtEmployeeLogin.Select("Id = " + "'" + s + "'")[0];
            row.BeginEdit();
            row["EmployeeStatus"] = statusEmployees;
            row["TimeStart"] = GetTime();
            row.EndEdit();
            SqlCommand commandUpdate = new SqlCommand();
            commandUpdate.Connection = con;
            commandUpdate.CommandType = CommandType.Text;
            commandUpdate.CommandText = @"Update dbo.Employee_Counter set EmployeeStatus = @EmployeeStatus, TimeStart =@TimeStart where Id =@Id ";

            commandUpdate.Parameters.Add("@Id", SqlDbType.NVarChar, 10, "Id");
            commandUpdate.Parameters.Add("@EmployeeStatus", SqlDbType.NVarChar, 10, "EmployeeStatus");
            commandUpdate.Parameters.Add("@TimeStart", SqlDbType.NVarChar, 20, "TimeStart");

            dtEmployee.UpdateCommand = commandUpdate;
            dtEmployee.Update(dtEmployeeLogin);
            //MessageBox.Show("Da cap nhat trang thai login + s ");
            dtEmployeeLogin.Clear();

        }

        private void StatusEmployeeLogout(string s)
        {
            DataRow row = dtEmployeeLogin.Select("Id = " + "'" + s + "'")[0];
            row.BeginEdit();
            row["EmployeeStatus"] = statusEmployees;
            row["TimeEnd"] = GetTime();
            row.EndEdit();

            SqlCommand commandUpdate = new SqlCommand();
            commandUpdate.Connection = con;
            commandUpdate.CommandType = CommandType.Text;
            commandUpdate.CommandText = @"Update dbo.Employee_Counter set EmployeeStatus = @EmployeeStatus, TimeEnd =@TimeEnd where Id =@Id ";

            commandUpdate.Parameters.Add("@Id", SqlDbType.NVarChar, 10, "Id");
            commandUpdate.Parameters.Add("@EmployeeStatus", SqlDbType.NVarChar, 10, "EmployeeStatus");
            commandUpdate.Parameters.Add("@TimeEnd", SqlDbType.NVarChar, 20, "TimeEnd");

            dtEmployee.UpdateCommand = commandUpdate;
            dtEmployee.Update(dtEmployeeLogin);
            //MessageBox.Show("Da cap nhat trang thai login + s ");
            dtEmployeeLogin.Clear();

        }
        
        private string GetTime()
        {
            DateTime tn = DateTime.Now;
            time = tn.ToString("dd/MM/yyyy HH:mm:ss");
            return time;
        }

        #region Lay kh chua phuc vu quay 2 dua vao queue2
        private void GetCustomer2() {
            int indexCustomer2 = 0;
            SqlCommand command = new SqlCommand(); // Khai bao 1 command
            command.Connection = con; // Ket noi
            command.CommandType = CommandType.Text; // Khai bao kieu command
            command.CommandText = @"select IdCustomer from dbo.Counter_Customer where SCounter = '2' and CStatus = 'False'";
            dCustomer2.SelectCommand = command;
            dCustomer2.Fill(dtCustomer2);           
            
            foreach (DataRow dr in dtCustomer2.Rows) {
                queue2.Enqueue(dr["IdCustomer"].ToString());
                ArrayCustomer2[indexCustomer2] = Convert.ToInt16(dr["IdCustomer"].ToString());
                indexCustomer2 = indexCustomer2 + 1;
            }
        }
        #endregion
        
        #region Lay kh chua phuc vu quay 2 dua vao queue3
        private void GetCustomer3()
        {

            SqlCommand command = new SqlCommand(); // Khai bao 1 command
            command.Connection = con; // Ket noi
            command.CommandType = CommandType.Text; // Khai bao kieu command
            command.CommandText = @"select IdCustomer from dbo.Counter_Customer where SCounter = '3' and CStatus = 'False'";
            dCustomer3.SelectCommand = command;
            dCustomer3.Fill(dtCustomer3);
            
            foreach (DataRow dr in dtCustomer3.Rows){
                queue3.Enqueue(dr["IdCustomer"].ToString());
            }

        }
        #endregion

        #region Cap nhat trang thai kh, s- id kh
        // Loi s = 0000 , khong co trong csdl
        private void UpdateStatusCustomer( string s){
            if (s.Length == 4 && s != "0000")
            {
                try
                {
                    DataRow row = dtCustomer.Select("IdCustomer = " + "'" + s + "'")[0];
                    row.BeginEdit();
                    row["CStatus"] = "True";
                    row.EndEdit();

                    SqlCommand commandUpdate = new SqlCommand();
                    commandUpdate.Connection = con;
                    commandUpdate.CommandType = CommandType.Text;
                    commandUpdate.CommandText = @"Update dbo.Counter_Customer set CStatus = @CStatus where IdCustomer = @IdCustomer ";
                    commandUpdate.Parameters.Add("@IdCustomer", SqlDbType.Int, 10, "IdCustomer");
                    commandUpdate.Parameters.Add("@CStatus", SqlDbType.Bit, 2, "CStatus");
                    da.UpdateCommand = commandUpdate;
                    da.Update(dtCustomer);
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Thông báo", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                }

            }
            else {
                //MessageBox.Show("Error Id khach hang ACK");
            }
        
        }
        #endregion

        #region Update gia tri 
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Cap nhat khach hang vao danh sach
            dtCustomer.Clear();
            GetCounterCustomer();

            // Lay ds kh dua vao queue
            GetCustomer2();
            GetCustomer3();

            // Lay so luong kh da phuc vu - dang phuc vu
            GetCustomerServed2();
            GetCustomerServeing2();

            GetCustomerServed3();
            GetCustomerServeing3();

            // Cap nhat sl kh vao quay
            SetCustomerServed("2", servedCus2,serveingCus2);
            SetCustomerServed("3", servedCus3,serveingCus3);
           
        }
        #endregion

        // Lay so luong kh da phuc vu
        private void GetCustomerServed2() {
            SqlCommand command = new SqlCommand(); // Khai bao 1 command
            command.Connection = con; // Ket noi
            command.CommandType = CommandType.Text; // Khai bao kieu command
            command.CommandText = @"select count(*) as 'Served2' from dbo.Counter_Customer where SCounter = '2' and CStatus = 'True' ";
            dCusServed2.SelectCommand = command;
            dCusServed2.Fill(dtCusServed2);
            foreach ( DataRow dr in dtCusServed2.Rows){
                servedCus2 = dr["Served2"].ToString();
            }        
        }

        // Lay so luong kh dang phuc vu
        private void GetCustomerServeing2() {
            SqlCommand command = new SqlCommand(); // Khai bao 1 command
            command.Connection = con; // Ket noi
            command.CommandType = CommandType.Text; // Khai bao kieu command
            command.CommandText = @"select count(*) as 'Serveing2' from dbo.Counter_Customer where SCounter = '2' and CStatus = 'False' ";
            dCusServeing2.SelectCommand = command;
            dCusServeing2.Fill(dtCusServeing2);
            foreach( DataRow dr in dtCusServeing2.Rows){
                serveingCus2 = dr["Serveing2"].ToString();
            }

        }

        private void GetCustomerServed3()
        {
            SqlCommand command = new SqlCommand(); // Khai bao 1 command
            command.Connection = con; // Ket noi
            command.CommandType = CommandType.Text; // Khai bao kieu command
            command.CommandText = @"select count(*) as 'Served3' from dbo.Counter_Customer where SCounter = '3' and CStatus = 'True' ";
            dCusServed3.SelectCommand = command;
            dCusServed3.Fill(dtCusServed3);
            foreach (DataRow dr in dtCusServed3.Rows)
            {
                servedCus3 = dr["Served3"].ToString();
            }
        }

        private void GetCustomerServeing3()
        {
            SqlCommand command = new SqlCommand(); // Khai bao 1 command
            command.Connection = con; // Ket noi
            command.CommandType = CommandType.Text; // Khai bao kieu command
            command.CommandText = @"select count(*) as 'Serveing3' from dbo.Counter_Customer where SCounter = '3' and CStatus = 'False' ";
            dCusServeing3.SelectCommand = command;
            dCusServeing3.Fill(dtCusServeing3);
            foreach (DataRow dr in dtCusServeing3.Rows)
            {
                serveingCus3 = dr["Serveing3"].ToString();
            }

        }

        #region Cap nhat so luong kh da phuc vu, idCounter - quay , count - kh
        private void SetCustomerServed( string idCounter, string countServed, string countServeing) {
            // Cap nhat vao quay
            try
            {
                DataRow row = dtCounter.Select("IdCounter = " + idCounter )[0];
                row.BeginEdit();
                row["CustomerServed"] = countServed;
                row["CustomerServeing"] = countServeing;
                row.EndEdit();

                SqlCommand commandUpdate = new SqlCommand();
                commandUpdate.Connection = con;
                commandUpdate.CommandType = CommandType.Text;
                commandUpdate.CommandText = @"Update dbo.Counter set CustomerServed = @CustomerServed, CustomerServeing= @CustomerServeing where IdCounter = @IdCounter ";
                commandUpdate.Parameters.Add("@IdCounter", SqlDbType.NVarChar, 10, "IdCounter");
                commandUpdate.Parameters.Add("@CustomerServed", SqlDbType.Int, 10, "CustomerServed");
                commandUpdate.Parameters.Add("@CustomerServeing", SqlDbType.Int, 10, "CustomerServeing");
                dCounter.UpdateCommand = commandUpdate;
                dCounter.Update(dtCounter);

            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString(), "Thông báo", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
            }
        }
        #endregion

        private void nhânViênLàmViệcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEmployee mEmployee = new frmEmployee();
            mEmployee.Show();
        }

        private void ThoiGianlamViecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTimeEmployee timeEmployee = new frmTimeEmployee();
            timeEmployee.Show();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dtCustomer.Clear();
            GetCounterCustomer();
        }

        private void báoCáoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReport report = new frmReport();
            report.Show();
        }

    }
}
