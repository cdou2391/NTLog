using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;


namespace NetLog
{

    public partial class frmLogin1 : Form
    {

        public frmLogin1()
        {
            InitializeComponent();
        }

        private void btnExitClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void logOutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmLogin LogIn = new frmLogin();
            this.Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string refNum = closeComboRefNo.Text;
            if (refNum.ToString().Length > 2)
            {
                using (SqlConnection conn = new SqlConnection(CheckDatabaseConnection.connectionStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * from calls where referenceNumber='" + refNum + "'", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        reader.Read();
                        closeTxtType.Text = reader["type"].ToString();
                        closeTxtDescription.Text = reader["description"].ToString();
                        closeTxtAssignedTo.Text = reader["staffEmail"].ToString();
                        closeTxtAssignedBy.Text = reader["assignedBy"].ToString();

                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid reference number");
            }
        }

        private void logOutToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frmLogin LogIn = new frmLogin();
            this.Close();
        }

        private void frmLogin1_Load(object sender, EventArgs e)
        {
            viewComboRefNumInput.Enabled = false;
            staffNames.Text = Globals.Staff.Name + " " + Globals.Staff.Surname;
            LoadCalls();
            DataTable table = new DataTable();
            string technician = Globals.Staff.Email;
            using (SqlConnection conn = new SqlConnection(CheckDatabaseConnection.connectionStr))
            {
                conn.Open();
                using (conn)
                {
                    SqlDataAdapter reader = new SqlDataAdapter("SELECT * from Tickets where staffEmail='"+ technician+ "'", conn);
                    DataSet ds = new DataSet();
                    reader.Fill(ds);
                    dataGridView1.VirtualMode = false;
                    dataGridView1.Columns.Clear();
                    dataGridView1.AutoGenerateColumns = true;
                    dataGridView1.DataSource = ds.Tables[0];
                    dataGridView1.Refresh();
                    try
                    {

                        for (int rows = 0; rows < dataGridView1.Rows.Count; rows++)
                        {
                            string status = dataGridView1.Rows[rows].Cells[6].Value.ToString();
                            if (status == "Closed")
                            {
                                dataGridView1.Rows[rows].DefaultCellStyle.BackColor = Color.Yellow;
                            }
                            else if (status == "Escalated")
                            {
                                dataGridView1.Rows[rows].DefaultCellStyle.BackColor = Color.Red;
                            }
                            else if (status == "Opened")
                            {
                                dataGridView1.Rows[rows].DefaultCellStyle.BackColor = Color.Green;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

        }
        private void LoadCalls()
        {
            //locComboUnikNo.Items.Clear();
            using (SqlConnection conn = new SqlConnection(CheckDatabaseConnection.connectionStr))
            {
                conn.Open();

                string technician = Globals.Staff.Email;
                using (SqlCommand cmd = new SqlCommand("SELECT * from Tickets WHERE staffEmail='" + technician + "'", conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ComboboxItem refNo = new ComboboxItem();
                        refNo.Text = reader["referenceNumber"].ToString();
                        viewComboRefNumInput.Items.Add(refNo);
                        //locComboUnikNo.Items.Add(refNo);
                        closeComboRefNo.Items.Add(refNo);
                    }
                }
            }

        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            //Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            string techinician = Globals.Staff.Name + " " + Globals.Staff;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            int i = 0;
            int j = 0;

            for (i = 0; i <= dataGridView1.RowCount - 1; i++)
            {
                for (j = 0; j <= dataGridView1.ColumnCount - 1; j++)
                {
                    DataGridViewCell cell = dataGridView1[j, i];
                    xlWorkSheet.Cells[i + 1, j + 1] = cell.Value;
                }
            }

            xlWorkBook.SaveAs(techinician+" Calls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

            MessageBox.Show("Excel file created , you can find the file in your document folder");
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string filter1 = viewComboIncReq.Text;
            DataTable table = new DataTable();

            string technician = Globals.Staff.Email;
            using (SqlConnection conn = new SqlConnection(CheckDatabaseConnection.connectionStr))
            {
                conn.Open();
                if ((filter1 == "Incident")|| (filter1 == "Request"))
                {
                    SqlDataAdapter reader = new SqlDataAdapter("SELECT * from calls WHERE type='"+filter1+"' and staffEmail='"+technician+"'", conn);
                    DataSet ds = new DataSet();
                    reader.Fill(ds);
                    dataGridView1.VirtualMode = false;
                    dataGridView1.Columns.Clear();
                    dataGridView1.AutoGenerateColumns = true;
                    dataGridView1.DataSource = ds.Tables[0];
                    dataGridView1.Refresh();
                    try
                    {

                        for (int rows = 0; rows < dataGridView1.Rows.Count; rows++)
                        {
                            string status = dataGridView1.Rows[rows].Cells[6].Value.ToString();
                            if (status == "Closed")
                            {
                                dataGridView1.Rows[rows].DefaultCellStyle.BackColor = Color.Yellow;
                            }
                            else if (status == "Escalated")
                            {
                                dataGridView1.Rows[rows].DefaultCellStyle.BackColor = Color.Red;
                            }
                            else if (status == "Opened")
                            {
                                dataGridView1.Rows[rows].DefaultCellStyle.BackColor = Color.Green;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                else if (viewComboIncReq.SelectedIndex==0)//(filter1=="Reference Number")
                {
                    viewComboRefNumInput.Enabled = true;
                    string refNum = viewComboRefNumInput.Text;
                    SqlDataAdapter reader = new SqlDataAdapter("SELECT * from calls WHERE referenceNumber='" + refNum + "'and staffEmail='" + technician + "'", conn);
                    DataSet ds = new DataSet();
                    reader.Fill(ds);
                    dataGridView1.VirtualMode = false;
                    dataGridView1.Columns.Clear();
                    dataGridView1.AutoGenerateColumns = true;
                    dataGridView1.DataSource = ds.Tables[0];
                    dataGridView1.Refresh();
                    try
                    {

                        for (int rows = 0; rows < dataGridView1.Rows.Count; rows++)
                        {
                            string status = dataGridView1.Rows[rows].Cells[6].Value.ToString();
                            if (status == "Closed")
                            {
                                dataGridView1.Rows[rows].DefaultCellStyle.BackColor = Color.Yellow;
                            }
                            else if (status == "Escalated")
                            {
                                dataGridView1.Rows[rows].DefaultCellStyle.BackColor = Color.Red;
                            }
                            else if (status == "Opened")
                            {
                                dataGridView1.Rows[rows].DefaultCellStyle.BackColor = Color.Green;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    using (conn)
                    {
                        
                        SqlDataAdapter reader = new SqlDataAdapter("SELECT * from calls WHERE staffEmail='" + technician + "'", conn);
                        DataSet ds = new DataSet();
                        reader.Fill(ds);
                        dataGridView1.VirtualMode = false;
                        dataGridView1.Columns.Clear();
                        dataGridView1.AutoGenerateColumns = true;
                        dataGridView1.DataSource = ds.Tables[0];
                        dataGridView1.Refresh();
                        try
                        {

                            for (int rows = 0; rows < dataGridView1.Rows.Count; rows++)
                            {
                                string status = dataGridView1.Rows[rows].Cells[6].Value.ToString();
                                if (status == "Closed")
                                {
                                    dataGridView1.Rows[rows].DefaultCellStyle.BackColor = Color.Yellow;
                                }
                                else if (status == "Escalated")
                                {
                                    dataGridView1.Rows[rows].DefaultCellStyle.BackColor = Color.Red;
                                }
                                else if (status == "Opened")
                                {
                                    dataGridView1.Rows[rows].DefaultCellStyle.BackColor = Color.Green;
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }

        private void viewComboIncReq_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (viewComboIncReq.SelectedIndex == 0)
            {
                viewComboRefNumInput.Enabled = true;
            }
            else
            {
                viewComboRefNumInput.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string callStatus = "Closed";
            string callDateResolved = DateTime.Now.ToString();
            string statusDescription = closeTxtMessage.Text;
            string refNum = closeComboRefNo.Text;

            using (SqlConnection conn = new SqlConnection(CheckDatabaseConnection.connectionStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("update calls set status= @callStatus ,description=@statusDescription, dateResolved= @dateResolved where referenceNumber = '" + refNum + "'", conn))
                {

                    cmd.Parameters.AddWithValue("@callStatus", callStatus);
                    cmd.Parameters.AddWithValue("@dateResolved", callDateResolved);
                    cmd.Parameters.AddWithValue("@statusDescription", statusDescription);


                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Saved");

                }
            }
        }
    }
    }
