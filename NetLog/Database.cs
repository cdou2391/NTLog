using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace NetLog
{
    class Database
    {
        public static string connectionStr = @"DATA SOURCE=Cedric; Initial Catalog=ntlog;UID=sa;PASSWORD=lbjames2391";
        public static SqlConnection connection = new SqlConnection(connectionStr);

        public static void Connect()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    // var ping = new Ping();
                    // var reply = ping.Send("google.com", 60 * 1000); // 1 minute time out (in ms)
                    //if (reply.Status.ToString().ToLower() == "success")
                    // {
                    connection.Open();
                    //  }

                }
                catch
                {
                    //MessageBox.Show("");
                }
            }
        }

        public static void Disconnect()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public static bool isOpen()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                return true;
            }
            else {
                return false;
            }
        }

    }
}
