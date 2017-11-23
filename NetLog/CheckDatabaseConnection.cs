using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace NetLog
{
    class CheckDatabaseConnection
    {
        public bool checkDatabase()
        {
            bool answer=true;
            using (var conn = new SqlConnection(Database.connectionStr))
            {
                try
                {
                    conn.Open();
                    answer = true;
                }
                catch(SqlException)
                {
                    answer = false;
                }

            }
            
           
            return answer;
        }
    }
}
