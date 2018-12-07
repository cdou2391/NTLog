using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace NetLog
{
    public partial class frmLogin : Form
    {

        public frmLogin()
        {
            InitializeComponent();
            txtEmail.Focus();
            
        }
        

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLog_Click_1(object sender, EventArgs e)
        {
            try
            {
                
                //if (connected == "true")
                //{
                    if (!string.IsNullOrWhiteSpace((txtEmail.Text)))
                    {
                        using (SqlConnection conn = new SqlConnection(CheckDatabaseConnection.connectionStr))
                        {
                            DataTable table = new DataTable();
                            SqlDataAdapter adapter = new SqlDataAdapter(@"select * from Staff where email = '" + txtEmail.Text + "' and password = '" + textBox2.Text + "'", conn);
                            adapter.Fill(table);
                            if (table.Rows.Count > 0)
                            {
                                if (table.Rows[0]["role"].ToString().Equals("admin", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Globals.Staff = new Staff
                                    {
                                        Name = table.Rows[0]["name"].ToString(),
                                        Email = table.Rows[0]["email"].ToString(),
                                        Gender = table.Rows[0]["gender"].ToString(),
                                        Password = table.Rows[0]["password"].ToString(),
                                        Role = table.Rows[0]["role"].ToString(),
                                        Surname = table.Rows[0]["surname"].ToString()
                                    };
                                    frmHome home = new frmHome();
                                    home.Show();
                                }
                                else if (table.Rows[0]["role"].ToString().Equals("2", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Globals.Staff = new Staff
                                    {
                                        Name = table.Rows[0]["name"].ToString(),
                                        Email = table.Rows[0]["email"].ToString(),
                                        Gender = table.Rows[0]["gender"].ToString(),
                                        Password = table.Rows[0]["password"].ToString(),
                                        Role = table.Rows[0]["role"].ToString(),
                                        Surname = table.Rows[0]["surname"].ToString()
                                    };

                                    frmLogin1 home = new frmLogin1();
                                    home.Show();
                                }
                                else
                                {
                                    MessageBox.Show("You are not an admin");
                                }
                                
                            }
                            else
                            {
                                MessageBox.Show("Please provide the correct username and passowrd");
                            }
                        }
                    }
                    else
                    {

                        MessageBox.Show("Please provide a username and password");

                    }

                //}
                //else
                //{
                //    throw new Exception("Connection to the database was not established.\r\n" +
                //                    "Please make sure that your database is on and that you are connected to the internet!");
                //}
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                new LogWriter(ex);
                this.Close();
            }
               
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                string connected;
                CheckDatabaseConnection check = new CheckDatabaseConnection();
                connected = check.checkDatabase();
                if(connected=="True")
                {
                    using (SqlConnection conn = new SqlConnection(CheckDatabaseConnection.connectionStr))
                    {
                        SqlCommand cmd = new SqlCommand("Select Email FROM Staff", conn);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();
                        while (reader.Read())
                        {
                            MyCollection.Add(reader.GetString(0));
                        }
                        txtEmail.AutoCompleteCustomSource = MyCollection;
                        conn.Close();
                    }
                }
                else
                {
                    throw new Exception("Connection to the database was not established.\r\n" +
                                   "Please make sure that your database is on and that you are connected to the internet!");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                new LogWriter(ex);
                this.Close();
            }
            
        }
        
    }
}
