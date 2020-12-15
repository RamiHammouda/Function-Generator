using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp2.Model;

namespace WpfApp2.ViewModel
{
    public class SettingInfor : INotifyPropertyChanged
    {

        public string mServer { get; set; }
        public string mPort { get; set; }
        public string mUserId { get; set; }
        public string mPassword { get; set; }
        public string mDatabaseName { get; set; }
        public string mTabName { get; set; }

        private MyDBEntity mCheckedDB;

        private bool _beautifulJson, _elementEnable;

        
        public bool mElementEnable
        {
            get { return _elementEnable; }
            set
            {
                if (_elementEnable != value)
                { _elementEnable = value; OnPropertyChanged("mElementEnable"); }
            }
        }

        public bool mBeautifulJson
        {
            get { return _beautifulJson; }
            set
            {
                if (_beautifulJson != value)
                { _beautifulJson = value; OnPropertyChanged("mBeautifulJson"); }
            }
        }

        private string _errorServer;
        public string mErrorServer
        {
            get { return _errorServer; }
            set
            {
                if (_errorServer != value)
                { _errorServer = value; OnPropertyChanged("mErrorServer"); }
            }
        }

        public MySqlConnection conn { get; set; }

        private SettingInfor()
        {

        }

        private static SettingInfor _instance = null;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static SettingInfor Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SettingInfor();
                return _instance;
            }
        }
        public void SetSetting(string server = "fgdb-f2-htw.selfhost.co", string port = "3306", string user = "hoale", string pword = "TestPW123!456", string dbname = "plc_data", string tabname = "plc_data", bool enable = false, bool beauty = true)
        {
            mServer = server;
            mPort = port;
            mUserId = user;
            mPassword = pword;
            mDatabaseName = dbname;
            mTabName = tabname;
            mElementEnable = enable;
            mBeautifulJson = beauty;
            //PropertyChanged += PrintSomeInfo;
            //OnPropertyChanged("Test");
        }

        private void PrintSomeInfo(object sender, EventArgs e)
        {
            Console.WriteLine($"Element Visible:{mElementEnable}, Beautiful Format:{mBeautifulJson}");
        }

        private bool _connectionTest = false;
        public bool mConnectionTest
        {
            get { return _connectionTest; }
            set
            {
                if (_connectionTest != value)
                {
                    _connectionTest = value; OnPropertyChanged("mConnectionTest");
                }
            }
        }
        //Move to use from DB instead
        public bool CheckConnection()
        {
            mErrorServer = "Checking..Pls wait for a moment";
            if (conn != null)
                conn.Close();

            string connStr = String.Format("server={0};user id={1}; password={2}; database={3}; port={4}; pooling=false",
                mServer, mUserId, mPassword, mDatabaseName, mPort);

            try
            {
                conn = new MySqlConnection(connStr);
                conn.Open();

                mCheckedDB = new MyDBEntity(this);
            }
            catch (MySqlException ex)
            {
                mErrorServer = ex.Message;
                mConnectionTest = false;
                return false;
            }
            //conn.Close();
            mErrorServer = String.Empty;
            mConnectionTest = true;
            return true;
        }

        //public bool ConnectionTest()
        //{
        //    mErrorServer = "Checking..Pls wait for a moment";
        //    MyDBEntity myCurrentDB = new MyDBEntity(this);
        //    mConnectionTest = myCurrentDB.CheckConnection();
        //    mErrorServer = myCurrentDB.Notify;
        //    return mConnectionTest;
        //}
        public MyDBEntity getCheckedDatabase()
        {
            return mCheckedDB;
        }

        public string GetConnectionString()
        {
            return $"server={mServer};user id={mUserId}; password={mPassword}; database={mDatabaseName}; port={mPort}; pooling=false";
        }

        public void PrintInfo()
        {
            Console.WriteLine(this.GetConnectionString());
        }
        public bool ReverseEditMode()
        {
            mElementEnable = !mElementEnable;
            return mElementEnable;
        }
    }
}
