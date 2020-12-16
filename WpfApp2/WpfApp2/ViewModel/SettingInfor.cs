using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp2.Model;

namespace WpfApp2.ViewModel
{
    public class SettingInfor : INotifyPropertyChanged
    {
        private string _port;
        [JsonProperty("Server", Order = 0)]
        public string mServer { get; set; }
        [JsonProperty("Port", Order = 1)]
        public string mPort
        {
            get { return _port; }
            set
            {
                if (_port != value)
                { _port = value; OnPropertyChanged("mPort"); }
            }
        }
        [JsonProperty("UserId", Order = 2)]
        public string mUserId { get; set; }
        [JsonProperty("Passwrord", Order = 3)]
        public string mPassword { get; set; }
        [JsonProperty("DatabaseName", Order = 4)]
        public string mDatabaseName { get; set; }
        [JsonProperty("TableName", Order = 5)]
        public string mTabName { get; set; }

        private MyDBEntity mCheckedDB;

        private bool _beautifulJson, _elementEnable;

        [JsonIgnore]
        public bool mElementEnable
        {
            get { return _elementEnable; }
            set
            {
                if (_elementEnable != value)
                { _elementEnable = value; OnPropertyChanged("mElementEnable"); }
            }
        }
        [JsonProperty("BeautyFormat", Order = 6)]
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
        [JsonIgnore]
        public string mErrorServer
        {
            get { return _errorServer; }
            set
            {
                if (_errorServer != value)
                { _errorServer = value; OnPropertyChanged("mErrorServer"); }
            }
        }
        [JsonIgnore]
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
        private void LoadSetting(SettingInfor aSetting)
        {
            mServer = aSetting.mServer;
            mPort = aSetting.mPort;
            mUserId = aSetting.mUserId;
            mPassword = aSetting.mPassword;
            mDatabaseName = aSetting.mDatabaseName;
            mTabName = aSetting.mTabName;
            mBeautifulJson = aSetting.mBeautifulJson;
            mFinalTargetList = aSetting.mFinalTargetList;
        }

        private bool _connectionTest = false;
        [JsonIgnore]
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

        private void CreateDatabase()
        {
            mCheckedDB = new MyDBEntity(this);
        }
        [JsonIgnore]
        public List<ColumnDBSelectableHelper> mSelectableTargetList { get; set; }
        public List<ColumnDBSelectableHelper> GetSelectableList()
        {

            mSelectableTargetList = new List<ColumnDBSelectableHelper>();

            foreach (string column in mCheckedDB.GetColumns())
            {

                mSelectableTargetList.Add(new ColumnDBSelectableHelper(true, column));
            }

            return mSelectableTargetList;
        }
        [JsonProperty("FinalTargetList", Order = 7)]
        public List<string> mFinalTargetList { get; set; }


        public List<string> LoadFinalTargetList()
        {
            if (!File.Exists(mFilePath))
                return LoadFinalTargetListFromComboBox();
            return LoadFinalTargetListFromFile();
        }
        public List<string> LoadFinalTargetListFromFile()
        {
            mFinalTargetList = new List<string>();

            var serializer = new JsonSerializer { Formatting = Formatting.Indented, TypeNameHandling = TypeNameHandling.Auto };
            using (StreamReader reader = File.OpenText(mFilePath))
            {
                var atest = serializer.Deserialize(reader, typeof(SettingInfor)) as SettingInfor;
                mFinalTargetList = atest.mFinalTargetList;
            }
            Console.WriteLine("CurrentPrint:");
            this.PrintInfo();
            this.PrintListInfor();
            return mFinalTargetList;
        }
        public List<string> LoadFinalTargetListFromComboBox()
        {
            mFinalTargetList = new List<string>();
            foreach (ColumnDBSelectableHelper target in mSelectableTargetList)
            {
                if (target.mIsSelected) mFinalTargetList.Add(target.mColumnName);
            }

            //foreach (string str in mFinalTargetList)
            //    Console.WriteLine(str); 
            return mFinalTargetList;
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
            mCheckedDB = new MyDBEntity(this);
            return mCheckedDB;
        }


        public override string ToString()
        {
            return $"Server:{mServer} Port:{mPort} UserId:{mUserId} Password:{mPassword} DatabaseName:{mDatabaseName} TableName:{mTabName}";
        }
        public void PrintInfo()
        {
            Console.WriteLine(ToString());
        }
        public bool ReverseEditMode()
        {
            mElementEnable = !mElementEnable;
            return mElementEnable;
        }

        public void SaveInfoToFile()
        {
            LoadFinalTargetListFromComboBox();

            //OK
            if (!CheckConnection())
                return;
            string basePath = Directory.GetCurrentDirectory() + "\\AppSetting";
            Directory.CreateDirectory(basePath);
            string fileName = "FGSetting";
            string filePath = basePath + "\\" + fileName + ".json";

            JsonSerializer serializer = new JsonSerializer { Formatting = Formatting.Indented, TypeNameHandling = TypeNameHandling.Auto };
            using (StreamWriter writer = File.CreateText(filePath)) { serializer.Serialize(writer, this); }

            CreateDatabase();
        }

        public void PrintListInfor()
        {
            foreach (string str in mFinalTargetList)
                Console.WriteLine(str);
        }
        public void LoadInfoToSetting()
        {
            mFilePath = Directory.GetCurrentDirectory() + "\\AppSetting\\FGSetting.json";
            if (!File.Exists(mFilePath))
            {
                this.SetSetting();
                mConnectionTest = true;
            }
            else
                LoadInfoFromFile();

            CreateDatabase();
        }
        private string mFilePath;
        public void LoadInfoFromFile()
        {
            var serializer = new JsonSerializer { Formatting = Formatting.Indented, TypeNameHandling = TypeNameHandling.Auto };
            using (StreamReader reader = File.OpenText(mFilePath))
            {
                var atest = serializer.Deserialize(reader, typeof(SettingInfor)) as SettingInfor;
                this.LoadSetting(atest);
            }

        }
    }
}
