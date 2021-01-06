using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;


namespace WpfApp2.Model
{
    /*
     * MySQL-Class connecting FrequencyGenerator with DB
    */
    public class MyDBEntity
    {
        #region Fields
        private string serverIP;
        private string portNumber;
        private string username;
        private string password;
        private string database;
        private string tableName;
        private string tar;
        public bool CanConnectToDB = false;
        public bool CanReadInDB = false;
        public bool NewIntIsInserted = false;
        public bool NewDoubleIsInserted = false;
        public bool NewuShortIsInserted = false;
        public bool NewsByteIsInserted = false;
        public bool NewLongIsInserted = true;
        #endregion

        #region Properties
        /// <summary>
        /// DB ConnectionString
        /// </summary>
        private string ConnectionString => "SERVER=" + serverIP + "; PORT =" + portNumber + "; DATABASE=" + database + ";" + "UID=" + username + ";" + "PASSWORD=" + password + ";";

        /// <summary>
        /// Return TimeStamp in MySQL Format
        /// </summary>
        public string TimeStamp => DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");

        #endregion

        #region Constructor

        /// <summary>
        /// Empty Constructor for testing purposes
        /// </summary>
        public MyDBEntity()
        {
            // Empty for testing purposes
        }

        /// <summary>
        /// Constructor with SettingTab-Object will set up the server and user login data for the ConnectionString
        /// </summary>
        /// <param name="setting">SettingTab Object</param>
        public MyDBEntity(ViewModel.SettingInfor setting)
        {
            this.serverIP = setting.mServer;
            this.portNumber = setting.mPort;
            this.username = setting.mUserId;
            this.password = setting.mPassword;
            this.database = setting.mDatabaseName;
            this.tableName = setting.mTabName;
        }
        #endregion

        #region Methods
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
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    CanConnectToDB = true;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Reading in the DB
        /// </summary>
        /// <param name="ConnectionString">ConnectionString delivered by ConnectionString Var</param>
        /// <param name="queryString">QueryString delivered by function Callin Reader</param>
        public List<string> Reader(string ConnectionString, string queryString)
        {
            List<string> columns = new List<string>();

            using (MySqlConnection cn = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand(queryString, cn);
                command.Connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columns.Add(reader.GetString(0));
                    }
                }
                CanReadInDB = true;
            }
            return columns;
        }

        /// <summary>
        /// Get all Coloumn names
        /// </summary>
        /// <returns>String List with the names of Coloumns</returns>
        public List<string> GetColumns()
        {
            List<string> columns = new List<string>();

            string queryString = $"SELECT `COLUMN_NAME` FROM `INFORMATION_SCHEMA`.`COLUMNS`  WHERE `TABLE_SCHEMA`= '{database}' AND `TABLE_NAME`= '{tableName}' ORDER BY table_name, ordinal_position; ";

            columns = Reader(ConnectionString, queryString);

            return columns;
        }

        /// <summary>
        /// Get all the Columns from DB
        /// </summary>
        /// <returns>List<string> object with all column-names in the DB.</returns>
        public Dictionary<string, string> GetData()
        {
            List<string> columns = new List<string>();
            List<string> newData = new List<string>();
            Dictionary<string, string> completeData = new Dictionary<string, string>();

            string queryString = $"SELECT `COLUMN_NAME` FROM `INFORMATION_SCHEMA`.`COLUMNS`  WHERE `TABLE_SCHEMA`= '{database}' AND `TABLE_NAME`= '{tableName}' ORDER BY table_name, ordinal_position;";
            string value = $"SELECT * FROM {database}.{tableName} ORDER BY id DESC LIMIT 1;";

            columns = Reader(ConnectionString, queryString);
            newData = Reader(ConnectionString, value);
            
            //Adding Data to the dictionary
            foreach (var elem in columns)
            {
                foreach (var item in newData)
                {
                    completeData.Add(elem, item);
                }
            }

            // TODO: Check foreach(var elem in newData){ if(elem == NULL){ elem = "0";} }

            return completeData;
        }

        // Add Whole Row:

        /// <summary>
        /// Adding the new data from given Dictionary<string, string> .
        /// </summary>
        /// <param name="newData">New Data as Dictionary<string, string></param>
        public void InsertRow(Dictionary<string, string> newData)
        {
            // Cut's out ID and TimeStamp columns

            newData.Remove("Id");
            newData.Remove("id");
            newData.Remove("ID");
            newData.Remove("iD");
            newData.Remove("Timestamp");
            newData.Remove("timeStamp");
            newData.Remove("TimeStamp");
            newData.Remove("timestamp");

            // Fullfill the MySQL String to put Values into correct TargetColumns
            string insertString = $"INSERT INTO {database}.{tableName} (TimeStamp, {string.Join(",", newData.Keys)}) VALUES ('{TimeStamp}',{string.Join(",", newData.Values)})";

            // Connect to DB and submit the Connectionstring with all the juicy values.
            Connect(ConnectionString, insertString);
        }


        // Single Input Actions:

        /// <summary>
        /// Insert Target Column
        /// </summary>
        /// <param name="tar">Target Column in the DB</param>
        public void InsertInTargetColumn(string tar)
        {
            this.tar = tar;
        }

        /// <summary>
        /// Insert int Values into DB
        /// </summary>
        /// <param name="columns">Insert Value as int</param>
        public void Insert(int value)
        {
            string queryString = $"INSERT INTO {database}.{tableName}(TimeStamp, {tar}) VALUES ('{TimeStamp}', '{value}')  ";

            Connect(ConnectionString, queryString);
            NewIntIsInserted = true;
        }

        /// <summary>
        /// Insert double Values into DB
        /// </summary>
        /// <param name="columns">Insert Value as double</param>
        public void Insert(double value)
        {
            string queryString = $"INSERT INTO {database}.{tableName}(TimeStamp, {tar}) VALUES ('{TimeStamp}', '{value}')  ";

            Connect(ConnectionString, queryString);
            NewDoubleIsInserted = true;
        }

        /// <summary>
        /// Insert long Values into DB
        /// </summary>
        /// <param name="columns">Insert Value as long</param>
        public void Insert(long value)
        {
            string queryString = $"INSERT INTO {database}.{tableName}(TimeStamp, {tar}) VALUES ('{TimeStamp}', '{value}')  ";

            Connect(ConnectionString, queryString);
            NewLongIsInserted = true;
        }

        /// <summary>
        /// Insert ushort Values into DB
        /// </summary>
        /// <param name="columns">Insert Value as ushort</param>
        public void Insert(ushort value)
        {
            string queryString = $"INSERT INTO {database}.{tableName}(TimeStamp, {tar}) VALUES ('{TimeStamp}', '{value}')  ";

            Connect(ConnectionString, queryString);
            NewuShortIsInserted = true;
        }

        /// <summary>
        /// Insert sbyte Values into DB
        /// </summary>
        /// <param name="columns">Insert Value as sbyte</param>
        public void Insert(sbyte value)
        {
            string queryString = $"INSERT INTO {database}.{tableName}(TimeStamp, {tar}) VALUES ('{TimeStamp}', '{value}')  ";

            Connect(ConnectionString, queryString);
            NewsByteIsInserted = true;
        }
        #endregion
    }
}
