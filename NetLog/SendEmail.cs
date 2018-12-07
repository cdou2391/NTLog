using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
namespace NetLog
{
    class SendEmail
    {
        public string sendEmail(string sender, string receiver1, string receiver2, string clientName, string callMessage, string referenceNum)
        {
            string staff = sender;
            string tech = receiver1;
            string client = receiver2;
            string clientNames = clientName;
            string callDesc = callMessage;
            string refNum = referenceNum;
            string message="";

                using (SqlConnection conn = new SqlConnection(CheckDatabaseConnection.connectionStr))
                {
                conn.Open();
                try
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * from Staff where email = '" + client + "'", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        reader.Read();
                        string clientOrgId = reader["organisationID"].ToString();
                        reader.Close();
                        using (SqlCommand cmd2 = new SqlCommand("SELECT * from organisation where orgId = '" + clientOrgId + "'", conn))
                        {
                            SqlDataReader reader2 = cmd2.ExecuteReader();
                            reader2.Read();
                            string clientOrgName = reader2["name"].ToString();
                            string orgEmail = "rugced23@gmail.com"; reader2["email"].ToString();
                            var fromAddress = new MailAddress("rugced23@gmail.com", "From Netmarks Technologies");
                            var toAddress = new MailAddress(tech, "To Netmarks Technologies Technichian");
                            var toAddress2 = new MailAddress(orgEmail, "From " + clientOrgName);
                            var toAddress3 = new MailAddress(client, "From " + clientOrgName);
                            string fromPassword = "lbjames2391";
                            string subject = "Call Request";
                            string body;
                            body = ("Email from " + Globals.Staff.Name + " on the behalf of " + clientNames + "\n"
                                 + callDesc + "\n" + "Your call reference number is " + refNum);
                            var smtp = new SmtpClient
                            {
                                Host = "smtp.gmail.com",
                                Port = 587,
                                EnableSsl = true,
                                DeliveryMethod = SmtpDeliveryMethod.Network,
                                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                                Timeout = 20000
                            };
                            using (var message1 = new MailMessage(fromAddress, toAddress)
                            {
                                Subject = subject,
                                Body = body,
                            })
                            using (var message2 = new MailMessage(fromAddress, toAddress2)
                            {
                                Subject = subject,
                                Body = body,
                            })
                            using (var message3 = new MailMessage(fromAddress, toAddress3)
                            {
                                Subject = subject,
                                Body = body,
                            })
                            {
                                ServicePointManager.ServerCertificateValidationCallback =
                                                delegate (object s, X509Certificate certificate,
                                                         X509Chain chain, SslPolicyErrors sslPolicyErrors)
                                                { return true; };
                                try
                                {
                                    smtp.Send(message1);
                                    MessageBox.Show("Email to " + toAddress + " was succesfully sent!");
                                }
                                catch (Exception ex)
                                { MessageBox.Show("Email to rec1" + toAddress + " was no sent! \nError: " + ex.Message); }
                                try
                                {
                                    smtp.Send(message2);
                                    MessageBox.Show("Email to " + toAddress2 + " was succesfully sent!");
                                }
                                catch (Exception ex)
                                { MessageBox.Show("Email to rec2" + toAddress2 + " was no sent! \nError: " + ex.Message); }
                                try
                                {
                                    smtp.Send(message3);
                                    MessageBox.Show("Email to " + toAddress3 + " was succesfully sent!");
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Email to rec3" + toAddress3 + " was no sent! \nError: " + ex.Message);
                                }
                                message = ("All emails succesfully sent");
                                //	
                            }
                        }
                    }
                }
                catch (InvalidOperationException ex)
                
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    new LogWriter(ex);
                }
            
            }
            return message;
           
        }
    }
}