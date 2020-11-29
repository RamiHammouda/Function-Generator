using System;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace WpfApp2.DBClass
{
/*
 * DBConnection-Class connecting FrequencyGenerator with DB
*/
    public class DBConnection
    {
        #region Fields and Properties

        /// <summary>
        /// Server IP as String
        /// </summary>
        protected string ServerIP { private get; set; }

        /// <summary>
        /// Port as Int
        /// </summary>
        protected int PortNumber { private get; set; }

        /// <summary>
        /// User as String
        /// </summary>
        protected string Username { private get; set; }

        /// <summary>
        /// Password as String
        /// </summary>
        protected string Password { private get; set; }

        /// <summary>
        /// DB Name as String
        /// </summary>
        protected string Database { private get; set; }

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

        public void Connect(string ConnectionString, string queryString)
        {
            using (MySqlConnection cn = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand(queryString, cn);              

                try
                {
                    command.Connection.Open();                              // Establish Connection to DB
                    // Implement  
                }
                catch
                {
                    // Implement Connection failed routine..
                }
            }
        }



        // DB Value Select

        // DB Value Insert

        // DB Value Update

        // DB Value Delete
        #endregion
    }
}
