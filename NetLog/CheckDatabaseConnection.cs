using System.Data.SqlClient;

namespace NetLog
{
    class CheckDatabaseConnection
    {
        public static string connectionStr = @"Data Source=DATACENTER03\CEDRICDB;Initial Catalog = TicketsDB; Integrated Security = True";
        public static SqlConnection connection = new SqlConnection(connectionStr);
        public string checkDatabase()
        {
            string answer = null;
            using (var conn = new SqlConnection(CheckDatabaseConnection.connectionStr))
            {
                try
                {
                    conn.Open();
                    answer = "True";
                }
                catch (SqlException sql)
                {
                    answer = "False" + sql.Message;
                }
            }
            return answer;
        }
    }
}
