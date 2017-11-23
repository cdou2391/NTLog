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
            textBox1.Focus();
            
        }
        

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLog_Click_1(object sender, EventArgs e)
        {
            bool connected;
            CheckDatabaseConnection check = new CheckDatabaseConnection();
            connected = check.checkDatabase();
            if (connected==true)
            {
                if (!string.IsNullOrWhiteSpace((textBox1.Text)))
                {
                    using (SqlConnection conn = new SqlConnection(Database.connectionStr))
                    {
                        DataTable table = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter(@"select * from staffUser where email = '" + textBox1.Text + "' and password = '" + textBox2.Text + "'", conn);
                        adapter.Fill(table);
                        if (table.Rows.Count > 0)
                        {
                            if (table.Rows[0]["role"].ToString().Equals("1", StringComparison.CurrentCultureIgnoreCase))
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

                    MessageBox.Show("please provide a username and password");

                }

            }
            else
            {
                MessageBox.Show("You do not have internet connection.Please connect to the Internet to continue");
            }
               
        }

    }
}
