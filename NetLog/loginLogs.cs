using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace NetLog
{
    class loginLogs
    {
        private string m_exePath = string.Empty;
        public loginLogs(string Names, string ID, string Em, string Pos)
        {
            try
            {
                string message = string.Format("Login Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                message += string.Format("Technician Names: {0}", Names);
                message += Environment.NewLine;
                message += string.Format("Technician ID: {0}", ID);
                message += Environment.NewLine;
                message += string.Format("Technician Email: {0}", Em);
                message += Environment.NewLine;
                message += string.Format("Technician Position: {0}", Pos);
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";

                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + @"NTLog\Logs\TicketloginLogs.txt";
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"NTLog\Logs"));

                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(message);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                new LogWriter(ex);
            }

        }
    } 
}
