using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace NetLog
{
    class GenerateClientNumber
    {
        //long num;
        string clientNumber;
        public string getClientNumber()
        {
            using (SqlConnection conn = new SqlConnection(CheckDatabaseConnection.connectionStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) from client", conn);
                object count =cmd.ExecuteScalar();
                string dateStr = DateTime.Now.ToString("yyMMdd");
                clientNumber = dateStr + (Convert.ToInt32(count) + 1).ToString("D2");
                int count2 = 0;
                do
                {
                    SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) from client where clientNumber='" + clientNumber + "'", conn);
                    count2 = (int)cmd2.ExecuteScalar();
                    if (count2 > 0)
                    {
                        clientNumber = (Convert.ToInt32(clientNumber) + 1).ToString();
                    }
                }
                while (count2 > 0);
            }
            return clientNumber;

        }
    }
}
