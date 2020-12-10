using System;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace WpfApp2.DBClass
{
/*
 * DBConnection-Class connecting FrequencyGenerator with DB
*/
    public class DBConnection
    {
        #region Fields
        protected string serverIP;
        protected int portNumber;
        protected string username;
        protected string password;
        protected string database;
        protected string tablename;
        protected string timestamp;
        protected string columnName;
        #endregion

        #region Properties
        /// <summary>
        /// Server IP as String
        /// </summary>
        public string ServerIP
        {
            get => serverIP;
            set => serverIP = value;
        }

        /// <summary>
        /// Port as Int
        /// </summary>
        public int PortNumber
        {
            get => portNumber;
            set => portNumber = value;
        }

        /// <summary>
        /// User as String
        /// </summary>
        public string Username
        {
            get => username;
            set => username = value;
        }

        /// <summary>
        /// Password as String
        /// </summary>
        public string Password
        {
            get => password;
            set => password = value;
        }

        /// <summary>
        /// DB Name as String
        /// </summary>
        public string Database
        {
            get => database;
            set => database = value;
        }

        /// <summary>
        /// Table name as string
        /// </summary>
        public string TableName
        {
            get => TableName;
            set => TableName = value;
        }

        /// <summary>
        /// Returns the actual TimeStamp in MySQL Format
        /// </summary>
        public string TimeStamp
        {
            get => timestamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffff");
        }

        /// <summary>
        /// Target Column on DB
        /// </summary>
        public string ColumnName
        {
            get => columnName;
            set => columnName = value;
        }

        /// <summary>
        /// DB ConnectionString
        /// </summary>
        private string ConnectionString
        {
            get
            {
                return "SERVER=" + ServerIP + "; PORT =" + PortNumber + "; DATABASE=" + Database + ";" + "UID=" + Username + ";" + "PASSWORD=" + Password + ";";
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set the Server IP Value
        /// </summary>
        /// <param name="ip">Enter FQDN or Server IP.</param>
        public void setServerIP(string ip)
        {
            ServerIP = ip;
        }

        /// <summary>
        /// Set the PortNumber Value
        /// </summary>
        /// <param name="port">Enter a specified MySQL Port Number.</param>
        public void setPortNumber(int port)
        {
            PortNumber = port;
        }

        /// <summary>
        /// Set the Username Value
        /// </summary>
        /// <param name="ip">Enter MySQL Username for Login purposes.</param>
        public void setUsername(string name)
        {
            Username = name;
        }

        /// <summary>
        /// Set the Password Value
        /// </summary>
        /// <param name="ip">Enter Password fitting to the MySQL-Username.</param>
        public void setPassword(string pw)
        {
            Password = pw;
        }

        /// <summary>
        /// Set the Name of the Database Value
        /// </summary>
        /// <param name="ip">Enter the Name of the Database.</param>
        public void setDatabase(string dbname)
        {
            Database = dbname;
        }

        /// <summary>
        /// Build the Connection supported by ConnectionString and queryString Vars.
        /// </summary>
        /// <param name="ConnectionString">Collection of Informations, including FQDN, User/PW, Portnumber, TargetDatabase</param>
        /// <param name="queryString"></param>
        public void Connect(string ConnectionString, string queryString)
        {
            using (MySqlConnection cn = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand(queryString, cn);              

                try
                {
                    command.Connection.Open();                              // Establish Connection to DB
                    command.ExecuteNonQuery();
                }
                catch
                {
                    Exception e = new Exception();
                    MessageBox.Show(e.Message);
                }
            }
        }

        /// <summary>
        /// Reading in the DB
        /// </summary>
        /// <param name="ConnectionString">ConnectionString delivered by ConnectionString Var</param>
        /// <param name="queryString">QueryString delivered by function Callin Reader</param>
        private List<Tuple<string, string>> Reader(string ConnectionString, string queryString)
        {
            List<Tuple<string, string>> columns = new List<Tuple<string, string>>();
            
            using (MySqlConnection cn = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand(queryString, cn);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Tuple<string, string> nameAndType = Tuple.Create<string, string>(reader.GetString(0), reader.GetString(1));
                        columns.Add(nameAndType);
                    }
                }
            }
            return columns;
        }

        /// <summary>
        /// Get all the Columns from DB
        /// </summary>
        /// <returns>List<string> object with all column-names in the DB.</returns>
        public List<Tuple<string, string>> GetColumns()
        {
            List<Tuple<string, string>> columns = new List<Tuple<string, string>>();

            string queryString = $"SELECT `COLUMN_NAME`, `DATA_TYPE` FROM `INFORMATION_SCHEMA`.`COLUMNS`  WHERE `TABLE_SCHEMA`= '{Database}' AND `TABLE_NAME`= '{TableName}' ORDER BY table_name, ordinal_position; ";

            Reader(ConnectionString, queryString);

            return columns;
        }

        // DB Value Insert
        public void Insert(List<string> columns)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string name in columns)
            {
                sb.Append($"{name}, ");
            }

            string queryString = $"INSERT INTO {Database}.{TableName}(TimeStamp, {sb.ToString()}) VALUES ({TimeStamp}, )  ";
            
            Connect(ConnectionString, queryString);
        }
        #endregion
    }
}
