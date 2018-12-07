using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Data.SqlClient;

namespace NetLog
{
    class GenerateReferenceNumber
    {
        string referenceNum;
        public string GetUniqueKey()
        { using (SqlConnection conn = new SqlConnection(CheckDatabaseConnection.connectionStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) from Tickets", conn);
                object count = cmd.ExecuteScalar();
                referenceNum =(1000 + (Convert.ToInt32(count) + 1)).ToString();
            }
            return referenceNum;
        }
    }
}
