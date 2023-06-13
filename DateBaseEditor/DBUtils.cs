using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateBaseEditor
{
    public class DBUtils
    {
        public static MySqlConnection GetDBConnection()
        {
            string host = "localhost";
            int port = 3306;
            string database = "mysqldatabase";
            string username = "root";
            string password = "98053076ESZABVip";

            return DBMySQLUtils.GetDBConnection(host, port, database, username, password);
        }
    }
}
