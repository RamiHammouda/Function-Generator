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
    /// <summary>
    /// a single ton class contains all Setting info for programm
    /// </summary>
    public class SettingInfor : INotifyPropertyChanged
    {
        private string _port, _server, _userId, _password, _databaseName, _tabName;
        [JsonProperty("Server", Order = 0)]
        public string mServer { get { return _server; } set { if (_server != value) { _server = value; OnPropertyChanged("mServer"); } } }
        [JsonProperty("Port", Order = 1)]
        public string mPort { get { return _port; } set { if (_port != value) { _port = value; OnPropertyChanged("mPort"); } } }
        [JsonProperty("UserId", Order = 2)]
        public string mUserId { get { return _userId; } set { if (_userId != value) { _userId = value; OnPropertyChanged("mUserId"); } } }
        [JsonProperty("Password", Order = 3)]
        public string mPassword { get { return _password; } set { if (_password != value) { _password = value; OnPropertyChanged("mPassword"); } } }
        [JsonProperty("DatabaseName", Order = 4)]
        public string mDatabaseName { get { return _databaseName; } set { if (_databaseName != value) { _databaseName = value; OnPropertyChanged("mDatabaseName"); } } }
        [JsonProperty("TableName", Order = 5)]
        public string mTabName { get { return _tabName; } set { if (_tabName != value) { _tabName = value; OnPropertyChanged("mTabName"); } } }

        private double _sampleRate;
        [JsonProperty("SampleRate", Order = 6)]
        public double mRate
        {
            get { return _sampleRate; }
            set
            {
                if (_sampleRate != value)
                { _sampleRate = value; OnPropertyChanged("mRate"); }
            }
        }
        private MyDBEntity mCheckedDB;

        private string mFilePath => Directory.GetCurrentDirectory() + "\\AppSetting\\FGSetting.json";

        private bool _beautifulJson, _elementEnable;

        [JsonIgnore]
        public bool mElementEnable { get { return _elementEnable; } set { if (_elementEnable != value) { _elementEnable = value; OnPropertyChanged("mElementEnable"); } } }
        [JsonProperty("BeautyFormat", Order = 7)]
        public bool mBeautifulJson { get { return _beautifulJson; } set { if (_beautifulJson != value) { _beautifulJson = value; OnPropertyChanged("mBeautifulJson"); } } }

        private string _errorServer;
        [JsonIgnore]
        public string mErrorServer { get { return _errorServer; } set { if (_errorServer != value) { _errorServer = value; OnPropertyChanged("mErrorServer"); } } }
        [JsonIgnore]
        public MySqlConnection conn { get; set; }
        private SettingInfor(){}

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
        /// <summary>
        /// Set parameters to Setting based on input
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <param name="user"></param>
        /// <param name="pword"></param>
        /// <param name="dbname"></param>
        /// <param name="tabname"></param>
        /// <param name="rate"></param>
        /// <param name="enable"></param>
        /// <param name="beauty"></param>
        public void SetSetting(string server = "fgdb-f2-htw.selfhost.co", string port = "3306", string user = "hoale", string pword = "TestPW123!456", string dbname = "plc_data", string tabname = "plc_data", double rate = 4, bool enable = false, bool beauty = true)
        {
            mServer = server;
            mPort = port;
            mUserId = user;
            mPassword = pword;
            mDatabaseName = dbname;
            mTabName = tabname;
            mRate = rate;
            mElementEnable = enable;
            mBeautifulJson = beauty;
        }
        /// <summary>
        /// Load setting's parameter from another setting object
        /// </summary>
        /// <param name="aSetting"></param>
        private void LoadSetting(SettingInfor aSetting)
        {
            mServer = aSetting.mServer;
            mPort = aSetting.mPort;
            mUserId = aSetting.mUserId;
            mPassword = aSetting.mPassword;
            mDatabaseName = aSetting.mDatabaseName;
            mTabName = aSetting.mTabName;
            mRate = aSetting.mRate;
            mBeautifulJson = aSetting.mBeautifulJson;
            mFinalTargetList = aSetting.mFinalTargetList;
        }

        private bool _connectionTest = false;
        [JsonIgnore]
        public bool mConnectionTest { get { return _connectionTest; } set { if (_connectionTest != value) { _connectionTest = value; OnPropertyChanged("mConnectionTest");}}}
        /// <summary>
        /// Check Connection for input database parameters
        /// </summary>
        /// <returns></returns>
        public bool CheckConnection()
        {
            mErrorServer = "Checking..Pls wait for a moment";
            if (conn != null) 
                conn.Close();

            string connStr = String.Format("server={0};user id={1}; password={2}; database={3}; port={4}; pooling=true",
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
            conn.Close();
            mErrorServer = String.Empty;

            mConnectionTest = mElementEnable ? true : false;
            return true;
        }
        /// <summary>
        /// Create database from current setting parameters
        /// </summary>
        private void CreateDatabase()
        {
            mCheckedDB = new MyDBEntity(this);
        }
        [JsonIgnore]
        public List<ColumnDBSelectableHelper> mSelectableTargetList { get; set; }
        /// <summary>
        /// Create Selectable List from List of columns from database, SelectableColumn is object which includes just a boolean along with a column string
        /// </summary>
        /// <returns></returns>
        public List<ColumnDBSelectableHelper> GetSelectableList()
        {
            mSelectableTargetList = new List<ColumnDBSelectableHelper>();

            foreach (string column in mCheckedDB.GetColumns())
            {
                if ((column.ToLower() == "id") || (column.ToLower() == "timestamp"))
                    mSelectableTargetList.Add(new ColumnDBSelectableHelper(false, column));
                else
                    mSelectableTargetList.Add(new ColumnDBSelectableHelper(true, column));
            }

            return mSelectableTargetList;
        }
        [JsonProperty("FinalTargetList", Order = 7)]
        public List<string> mFinalTargetList { get; set; }
        /// <summary>
        /// This method helps to load chosen columns from combox box or from file (if exists)
        /// </summary>
        /// <returns>And return a list of columns as string</returns>
        public List<string> LoadFinalTargetList()
        {
            if (!File.Exists(mFilePath))
                return LoadFinalTargetListFromComboBox();
            return LoadFinalTargetListFromFile();
        }

        /// <summary>
        /// This method helps to load target columns on database from saved file
        /// </summary>
        /// <returns></returns>
        public List<string> LoadFinalTargetListFromFile()
        {
            mFinalTargetList = new List<string>();

            var serializer = new JsonSerializer { Formatting = Formatting.Indented, TypeNameHandling = TypeNameHandling.Auto };
            using (StreamReader reader = File.OpenText(mFilePath))
            {
                var atest = serializer.Deserialize(reader, typeof(SettingInfor)) as SettingInfor;
                mFinalTargetList = atest.mFinalTargetList;
            }
            return mFinalTargetList;
        }
        /// <summary>
        /// This method helps to load target columns on databas from combobox
        /// </summary>
        /// <returns></returns>
        public List<string> LoadFinalTargetListFromComboBox()
        {
            mFinalTargetList = new List<string>();
            foreach (ColumnDBSelectableHelper target in mSelectableTargetList)
            {
                if (target.mIsSelected) mFinalTargetList.Add(target.mColumnName);
            }
            return mFinalTargetList;
        }
        /// <summary>
        /// as its name described :)
        /// </summary>
        /// <returns></returns>
        public MyDBEntity getCheckedDatabase()
        {
            mCheckedDB = new MyDBEntity(this);
            return mCheckedDB;
        }

        /// <summary>
        /// Get essential setting info
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Server:{mServer} Port:{mPort} UserId:{mUserId} Password:{mPassword} DatabaseName:{mDatabaseName} TableName:{mTabName}";
        }
        /// <summary>
        /// Print out some setting information
        /// </summary>
        public void PrintInfo()
        {
            Console.WriteLine(ToString());
        }
        /// <summary>
        /// Reverse Edit-Mode. If Edit is enable, then disable, and vice versa
        /// </summary>
        /// <returns>boolean value, "true" is enable</returns>
        public bool ReverseEditMode()
        {
            mElementEnable = !mElementEnable;
            return mElementEnable;
        }
        /// <summary>
        /// When finish choosing target columns for database in comboxbox, it wont take effect immediately. Fixing that bug is reason of life for this method.
        /// </summary>
        private void ForceChangeToMainWindows()
        {
            //foreach (Window window in Application.Current.Windows)
            //{
            //    if (window.GetType() == typeof(MainWindow))
            //    {
            //        (window as MainWindow).lblwelcome.Content = "I changed it from another window";
            //    }
            //}

            var targetWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            //targetWindow.lblwelcome.Content = "Tesst";
            targetWindow.mMyTargetOnDB = mFinalTargetList;
            targetWindow.mRate = mRate;
        }
        /// <summary>
        /// Save setting Information to json file at current executable file
        /// </summary>
        public void SaveInfoToFile()
        {
            LoadFinalTargetListFromComboBox();
            ForceChangeToMainWindows();
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
        /// <summary>
        /// For testing only - Print string in list
        /// </summary>
        /// <param name="alist"></param>
        public void PrintListInfor(List<string> alist)
        {
            Console.WriteLine(alist.ToString());
            foreach (string str in alist)
                Console.WriteLine(str);
        }
        /// <summary>
        /// Load setting infor to currrent Setting entity. If file exists, then load from file, if not, then create setting information with default values
        /// </summary>
        public void LoadInfoToSetting()
        {
            if (!File.Exists(mFilePath))
            {
                this.SetSetting();
                mConnectionTest = true;
            }
            else
                LoadInfoFromFile();

            CreateDatabase();
        }
        
        /// <summary>
        /// Load setting information from json file
        /// </summary>
        public void LoadInfoFromFile()
        {
            var serializer = new JsonSerializer { Formatting = Formatting.Indented, TypeNameHandling = TypeNameHandling.Auto };
            using (StreamReader reader = File.OpenText(mFilePath))
            {
                this.LoadSetting(serializer.Deserialize(reader, typeof(SettingInfor)) as SettingInfor);
            }

        }
    }
}
