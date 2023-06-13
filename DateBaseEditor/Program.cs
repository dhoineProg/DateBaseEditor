using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data;

namespace DateBaseEditor
{
    public class Program
    {
        private static int id; 
        private static string name; 
        private static string email; 
        private static string password; 
        private static string nameTable; 
        public static string line = new string('_', 60);
        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в редактор таблиц базы данных!");
            Console.WriteLine();
            Console.WriteLine("Getting Connection ...");
            MySqlConnection conn = DBUtils.GetDBConnection();

            try
            {
                Console.WriteLine("Openning Connection ...");
                conn.Open();
                Console.WriteLine("Connection successful!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            Console.WriteLine();
            Console.WriteLine("Выберите действие, которое хотите выполнить: ");
            Console.WriteLine("1. Создать таблицу.");
            Console.WriteLine("2. Добавить нового пользователя в уже существующую таблицу.");
            Console.WriteLine("3. Получить список всех пользователей из таблицы.");
            Console.WriteLine("4. Обновить данные о пользователе в таблице.");
            Console.WriteLine("5. Удалить пользователя из таблицы.");
            Console.WriteLine("6. Удалить таблицу.");
            Console.WriteLine(line);
            int action = Int32.Parse(Console.ReadLine());
            switch (action) // Выбор метода, который будет выполняться
            {
                case 1:
                    CreateNewTable(); // 1. Создать таблицу.
                    break;
                case 2:
                    AddUserToTable(); // 2. Добавить нового пользователя в уже существующую таблицу.
                    break;
                case 3:
                    ShowTable(); // 3. Получить список всех пользователей из таблицы.
                    break;
                case 4:
                    UpdateTableData(); // 4. Обновить данные о пользователе в таблице.
                    break;
                case 5:
                    DeleteRow(); // 5. Удалить пользователя из таблицы.
                    break;
                case 6:
                    DeleteTable(); // 6. Удалить таблицу.
                    break;
            }

            Console.Read();
        }

        public static void CreateNewTable() // Метод создания новой таблицы
        {
            MySqlConnection conn = DBUtils.GetDBConnection(); // Подключение к базе данных
            
            try
            {
                conn.Open(); // Открываем соединение
                MySqlCommand command = conn.CreateCommand();
                Console.WriteLine("Введите название таблицы: ");
                nameTable = Convert.ToString(Console.ReadLine());
                // Запрос mySQL для создания новой таблицы
                command.CommandText = $@"CREATE TABLE IF NOT EXISTS {nameTable}( 
                                        Id MEDIUMINT AUTO_INCREMENT,
                                        Name CHAR(20) NOT NULL,                     
                                        Email CHAR(30) NOT NULL,
                                        Password CHAR(10) NOT NULL,
                                        PRIMARY KEY (Id));";
                
                command.ExecuteNonQuery();
                Console.WriteLine("Таблица создана.");
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            
            return;
        }

        public static void AddUserToTable() // Метод добавления новой записи в таблицу
        {
            Console.WriteLine("Введите название таблицы, которую нужно редактировать:");
            string nameTable = Convert.ToString(Console.ReadLine());
                Console.WriteLine("Введите Id пользователя: ");
                id = Int32.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine("Введите Имя пользователя: ");
                name = Convert.ToString(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine("Введите Email пользователя: ");
                email = Convert.ToString(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine("Введите пароль для пользователя: ");
                Console.WriteLine("Пароль должен быть не менее 0 и не более 10 символов! ");
                password = Convert.ToString(Console.ReadLine());
                if (password.Length > 10 || password.Length <= 0)
                {
                    Console.WriteLine("Пароль должен быть не меньше 0 и не превышать 10 символов.");
                    Console.WriteLine("Перезапустите программу.");
                    Console.ReadKey();
                }
                string sqlExpression = $"INSERT INTO {nameTable}(Id,Name,Email,Password)  VALUES ({id}, '{name}', '{email}', '{password}')"; //Запрос mySQL для добавления новой записи в таблицу
                try
                {
                    using (MySqlConnection connection = DBUtils.GetDBConnection())
                    {
                        connection.Open(); // Открытие соединения
                        MySqlCommand command = new MySqlCommand(sqlExpression, connection);
                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine("Пользователь добавлен.");
                }
                catch (MySqlException e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
                Console.ReadLine();
        }

        public static void ShowTable() // Метод вывод списка всех пользователей из таблицы
        {
            Console.WriteLine("Введите название таблицы, из которой необходимо получить список всех пользователей: ");
            string nameTable = Convert.ToString(Console.ReadLine());

            string query = $"SELECT * FROM {nameTable}";
            try
            {
                using (MySqlConnection conn = DBUtils.GetDBConnection())
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(query,conn);
                    MySqlDataReader reader = command.ExecuteReader();

                    DataTable dataTable = reader.GetSchemaTable(); //Получение информации о столбцах
                    List<string> coloumnNames = new List<string>();
                    foreach(DataRow row in dataTable.Rows)
                    {
                        coloumnNames.Add(row["ColumnName"].ToString());
                    }
                    while (reader.Read())
                    {
                        for (int i = 0; i < coloumnNames.Count; i++) Console.WriteLine("{0}:{1}", coloumnNames[i], reader[i]);
                    }
                    reader.Close();
                }

            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void UpdateTableData() // Метод обновления записи в таблице
        {
            try
            {
                Console.WriteLine("Введите таблицу, которую необходимо редактировать: ");
                string nameTable = Convert.ToString(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine("Выберите пользователя, который требует изменений, используя его id: ");
                int _id = Int32.Parse(Console.ReadLine());
                Console.WriteLine("");
                Console.WriteLine("Введите новое имя: ");
                string _name = Convert.ToString(Console.ReadLine());
                Console.WriteLine("");
                Console.WriteLine("Введите новый email: ");
                string _email = Convert.ToString(Console.ReadLine());   
                Console.WriteLine("");
                Console.WriteLine("Введите новый пароль(не менее 0 и не более 10 символов!): ");
                string _password = Convert.ToString(Console.ReadLine());
                if (_password.Length > 10 || _password.Length <= 0)
                {
                    Console.WriteLine("Пароль должен быть не меньше 0 и не превышать 10 символов.");
                    Console.WriteLine("Перезапустите программу.");
                    Console.ReadKey();
                }
                Console.WriteLine();
                string query = $"UPDATE {nameTable} SET Name=@name, Email=@email, Password=@password WHERE Id=@_id";

                MySqlConnection connection = DBUtils.GetDBConnection();

                using (MySqlCommand command = new MySqlCommand(query,connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@name",_name);
                    command.Parameters.AddWithValue("@email", _email);
                    command.Parameters.AddWithValue("@password", _password);
                    command.Parameters.AddWithValue("@_id", _id);
                    command.ExecuteNonQuery();

                    Console.WriteLine("Данные изменены.");
                }

            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void DeleteRow() // Метод удаления записи в таблице
        {
            Console.WriteLine("Введите таблицу, в которой необходимо внести изменения: ");
            string nameTable = Convert.ToString(Console.ReadLine());
            Console.WriteLine();
            Console.WriteLine("Введите номер id пользователя, которого необходимо удалить: ");
            int _id = Int32.Parse(Console.ReadLine());
            string queryDelete = $"DELETE FROM {nameTable} WHERE Id=@_id";

            MySqlConnection connection = DBUtils.GetDBConnection();
            try
            {
                using (MySqlCommand command = new MySqlCommand(queryDelete, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("_id", _id);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Запись удалена.");
                }
            } catch(MySqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void DeleteTable()
        {
            Console.WriteLine("Введите таблицу, которую необходимо удалить: ");
            string nameTable = Convert.ToString(Console.ReadLine());
            string queryDeleteTable = $"DROP TABLE {nameTable}";

            MySqlConnection connection = DBUtils.GetDBConnection();
            try
            {
                using(MySqlCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = queryDeleteTable;
                    command.ExecuteNonQuery();
                    Console.WriteLine("Таблица удалена.");
                }
            }catch(MySqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
