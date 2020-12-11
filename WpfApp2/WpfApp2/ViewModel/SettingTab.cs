using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp2.ViewModel
{
    public class SettingTab: INotifyPropertyChanged
    {
        
        public string mServer { get; set; }
        public string mPort { get; set; }
        public string mUserId { get; set; }
        public string mPassword { get; set; }
        public string mDatabaseName { get; set; }
        public string mTabName{ get; set; }

        public bool mElementVisible { get; set; }
        public bool mBeautifulJson { get; set; }

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

        private SettingTab()
        {

        }

        private static SettingTab _instance = null;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static SettingTab Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SettingTab();
                return _instance;
            }
        }
        public void SetSetting(string server="fgdb-f2-htw.selfhost.co", string port="3306", string user="hoale", string pword= "TestPW123!456", string dbname="plc_data", string tabname="plc_data",bool visible = false, bool beauty = true)
        {
            mServer = server;
            mPort = port;
            mUserId = user;
            mPassword = pword;
            mDatabaseName = dbname;
            mTabName = tabname;
            mElementVisible = visible;
            mBeautifulJson = beauty;
        }

        public bool CheckConnection()
        {
            mErrorServer = "Checking...";
            if (conn != null)
                conn.Close();

            string connStr = String.Format("server={0};user id={1}; password={2}; database={3}; port={4}; pooling=false",
                mServer, mUserId, mPassword, mDatabaseName,mPort);

            try
            {
                conn = new MySqlConnection(connStr);
                conn.Open();

            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.Message,"Oopps...", MessageBoxButton.OK, MessageBoxImage.Error);
                mErrorServer = ex.Message;
                return false;
            }
            conn.Close();
            mErrorServer = String.Empty;
            return true;
        }

        public string GetConnectionString()
        {
            return $"server={mServer};user id={mUserId}; password={mPassword}; database={mDatabaseName}; port={mPort}; pooling=false";
        }


    }
}
