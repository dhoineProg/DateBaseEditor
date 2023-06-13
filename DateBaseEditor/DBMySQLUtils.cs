using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateBaseEditor
{
    public class DBMySQLUtils
    {
        public static MySqlConnection GetDBConnection(string host, int port, string database, string username, string password)
        {
            string connString = "Server=" + host + ";DataBase=" + database + ";port=" + port + ";User ID=" + username + ";Password=" + password;

             MySqlConnection conn = new MySqlConnection(connString);

            return conn;
        }
    }
}
