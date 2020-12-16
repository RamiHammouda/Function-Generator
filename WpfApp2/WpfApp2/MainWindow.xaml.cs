﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using WpfApp2.Model;
using WpfApp2.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;


namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged, IDataErrorInfo
    {

        public MainWindow()
        {
            InitializeComponent();

            initiateDefaultValue();

        }


        #region Properties For Testing Only
        //public ListBindableAttribute targetFromDB = new ListBindableAttribute();
        public bool exportingIsFinished;
        public bool offsetFreqValueIncreased = false;
        public bool offsetFreqValueDecreased = false;
        public bool offsetAmpValueIncreased = false;
        public bool offsetAmpValueDecreased = false;
        public bool ItemAdded = false;
        public bool ItemDeleted = false;
        public bool ValidInput = false;
        public int n;
        public bool mValidInput { get; set; }
        #endregion

        #region Properties

        private string _errorMessage;
        public string mErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (_errorMessage != value)
                { _errorMessage = value; OnPropertyChanged("mErrorMessage"); }
            }
        }

        [Range(1, 4, ErrorMessage = "Sending Data Rate to Database is limitted to maximum 4 : 4 times/s")]
        public long mRate;
        public long mDuration;
        public double mOffsetAmpl;
        public double mOffsetFreq { get; set; }

        private WaveForm _wave;
        public WaveForm mWave
        {
            get { return _wave; }
            set
            {
                if (_wave != value)
                { _wave = value; OnPropertyChanged("mWave"); }
            }
        }

        private double _freq;
        [Range(0.0001, 4, ErrorMessage = "Frequency must from {1} to {2}")]
        [Required(ErrorMessage = "Frequency is required")]
        public double mFreq
        {
            get { return _freq; }
            set
            {
                if (_freq != value)
                { _freq = value; OnPropertyChanged("mFreq"); }
            }
        }
        private double _ampl;
        //[Range(0.001, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        [Range(0.001, Double.PositiveInfinity, ErrorMessage = "Amplitude must from {1}")]
        [Required(ErrorMessage = "Amplitude is required")]
        public double mAmpl
        {
            get { return _ampl; }
            set
            {
                if (_ampl != value)
                { _ampl = value; OnPropertyChanged("mAmpl"); }
            }
        }

        public SettingInfor mSettingTab { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<GenerateSignalData> _multipleShotList;
        public ObservableCollection<GenerateSignalData> mMultipleShotList
        {
            get { return _multipleShotList; }
            set
            {
                if (_multipleShotList != value)
                { _multipleShotList = value; OnPropertyChanged("_multipleShotList"); }
            }
        }

        public List<string> mMyTargetOnDB { get; set; }

        private string _selectedTargetOnDB;
        public string mSelectedTargetOnDB
        {
            get { return _selectedTargetOnDB; }
            set
            {
                if (_selectedTargetOnDB != value)
                { _selectedTargetOnDB = value; OnPropertyChanged("mSelectedTargetOnDB"); }
            }
        }


        string IDataErrorInfo.Error => throw new NotImplementedException(); //Part of Annotation

        string IDataErrorInfo.this[string propertyName] { get { return OnValidate(propertyName); } } //Part of Annotation

        protected virtual string OnValidate(string propertyName) //Part of Annotation
        {
            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentException("Property may not be null or emtpy", propertyName);

            string error = string.Empty;
            var value = this.GetType().GetProperty(propertyName).GetValue(this, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>(1);
            var context = new ValidationContext(this, null, null) { MemberName = propertyName };
            var result = Validator.TryValidateProperty(value, context, results);

            if (!result)
            {
                var validationResult = results.First();
                error = validationResult.ErrorMessage;
            }
            mErrorMessage = error;
            return error;
        }

        public ColumnDBSelectableHelper originalColumnWithSelection { get; set; }

        #endregion

        #region Some Control Element
        private void sldFreq_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            mOffsetFreq = Math.Round((sender as Slider).Value / 1000, 3);
        }

        private void sldAmp_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mOffsetAmpl = Math.Round((sender as Slider).Value / 1000, 3);
        }

        public void btnFreqPlus_Click(object sender, RoutedEventArgs e)
        {
            mFreq += mOffsetFreq;
            offsetFreqValueIncreased = true;
        }

        public void btnFreqMinus_Click(object sender, RoutedEventArgs e)
        {
            mFreq -= mOffsetFreq;
            offsetFreqValueDecreased = true;
        }

        public void btnAmplPlus_Click(object sender, RoutedEventArgs e)
        {
            mAmpl += Math.Round(mOffsetAmpl, 3);
            offsetAmpValueIncreased = true;
        }
        public void btnAmplMinus_Click(object sender, RoutedEventArgs e)
        {
            mAmpl -= Math.Round(mOffsetAmpl, 3);
            offsetAmpValueDecreased = true;
        }

        #endregion

        #region Functions and Buttons
        public void btnReset_Click(object sender, RoutedEventArgs e)
        {
            initiateDefaultValue();
        }

        public void initiateDefaultValue()
        {
            this.DataContext = this;

            mFreq = 0.2;
            mAmpl = 5;
            mRate = 4;
            mDuration = 0;
            mWave = WaveForm.Sine;
            mValidInput = false;
            mCurrentProfile = new GenerateSignalData(mWave, mFreq, mAmpl, mRate, mDuration);
            this.PropertyChanged += AutoDrawing;
            OnPropertyChanged("IamIronMan");

            _multipleShotList = new ObservableCollection<GenerateSignalData>()
            { new GenerateSignalData(),
              new GenerateSignalData(0,2,7,4,2,sendToDb:true),
              new GenerateSignalData(WaveForm.Random,targetOnDb:"Inputs_Entschlammung1_Status"),
              new GenerateSignalData(WaveForm.Sawtooth,sendToDb:true,targetOnDb:"Inputs_TestVarLReal")
            };

            this.mSettingTab = SettingInfor.Instance;

            mSettingTab.LoadInfoToSetting();
            if (!mSettingTab.CheckConnection())
            {
                cbbTargetList.ItemsSource = _offLineTargetList;
                mMyTargetOnDB = _offLineList;
            }
            else
            {
                cbbTargetList.ItemsSource = mSettingTab.GetSelectableList();
                mMyTargetOnDB = mSettingTab.LoadFinalTargetList();
            }


            //Because all elemente of Settingtab already has datacontext to SettingTab (inXml)
            //so cbb must set Datacontext to return to this class
            //cbbTargetList.DataContext = this;
            //txtCombobox.DataContext = this;

            //mMyTargetOnDB = GetTargetOnDB();


            //not quite follow Binding Rule, but simple and practical :)
            sldFreq.Value = 0;
            sldAmp.Value = 0;

            //only for Image Connection Result
            resultImg.DataContext = this;
            //Uri myUri = new Uri(@"/Images/icons8-ok-48.png", UriKind.Relative);
            //Uri myUri2 = new Uri("/WpfApp2;component/Images/icons8-ok-48.png",UriKind.Relative);
            //resultImg.Source = new BitmapImage(myUri2);
            //mUriImage = myUri;
        }
        private List<ColumnDBSelectableHelper> _offLineTargetList { get { List<ColumnDBSelectableHelper> aList = new List<ColumnDBSelectableHelper>() { new ColumnDBSelectableHelper(true, "Offline"), new ColumnDBSelectableHelper(true, "Just Kidding") }; return aList; } }
        private List<string> _offLineList { get { List<string> aList = new List<string>() { "Offline", "Offline only" }; return aList; } }
        private void VisualizateData(SimulationProfile smProfile, int numberOfWave)
        {
            if (!smProfile.checkedSmProfValidation())
                return;
            double displayduration = (double)numberOfWave / smProfile.getFreq() + (double)1 / smProfile.getRate();
            GenerateSignalData displayData = new GenerateSignalData(smProfile.getSignalProfile(), displayduration, true);
            double linewidth = 1, marksize = 5;
            string text = $"Wave: {displayData.getWave()}\nFreq: {displayData.getFreq()} Hz\nAmpl: {displayData.getAmpl()} V\nRate: {displayData.getRate()}\nDura: {smProfile.getDuration()} s";
            double[] x = GenerateSignalData.ConvertToDouble(displayData.getNo().ToArray());
            double[] y = displayData.getAmplData().ToArray();

            if ((double)displayData.getRate() / displayData.getFreq() > 50)
            {
                linewidth = 2;
                marksize = 0;
            }

            myWpfPlot.plt.Clear();
            myWpfPlot.plt.PlotScatter(x, y, lineWidth: linewidth, markerSize: marksize, label: text);
            myWpfPlot.plt.Style(ScottPlot.Style.Light2);
            myWpfPlot.plt.Title("Signal Data", fontName: "Verdana", color: Color.BlueViolet, bold: true);
            myWpfPlot.plt.YLabel("Amplitude", fontSize: 16, color: Color.Green);
            myWpfPlot.plt.XLabel("Time(Ticks)", color: Color.Green);
            myWpfPlot.plt.Style(dataBg: Color.LightYellow);
            myWpfPlot.plt.PlotAnnotation(text, -10, 10, fontSize: 9);

            try
            {
                myWpfPlot.Render();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        private GenerateSignalData mCurrentProfile, mRunningProfile;
        private void AutoDrawing(object sender, EventArgs e)
        {
            mCurrentProfile = new GenerateSignalData(mWave, mFreq, mAmpl, mRate, mDuration);
            SimulationProfile currentSignal = mCurrentProfile.getSimulationProfile();
            VisualizateData(currentSignal, 4);
            //Console.WriteLine(mSelectedTargetOnDB);
            //Console.WriteLine(mCurrentProfile.ToString());
            //Console.WriteLine(mSelectedRowOnDataGrid == null ? "Not yet selected" : $"{mSelectedRowOnDataGrid.mTargetOnDB}");
        }
        private bool _singleShotPressed, _multipleShotPressed;
        private void btnSimulate_Click(object sender, RoutedEventArgs e)
        {
            if (mSelectedTargetOnDB == null)
            {
                mErrorMessage = "Pls choose Target first";
                return;
            }
            if (_multipleShotPressed)
            {
                mErrorMessage = "Multiple Shot is running!!";
                return;

            }
            mErrorMessage = String.Empty;
            //mCurrentDatabase = new MyDBEntity(mSettingTab);

            _singleShotPressed = !_singleShotPressed;
            if (_singleShotPressed)
            {
                try
                {
                    if (!mSettingTab.CheckConnection())
                    {
                        mErrorMessage = "Connection Test is fail";
                        return;
                    }
                    mCurrentDatabase = mSettingTab.getCheckedDatabase();
                    mCurrentProfile.setMyDB(mCurrentDatabase);
                    mCurrentProfile.setTargetOnDB(mSelectedTargetOnDB);

                    mRunningProfile = mCurrentProfile;
                    btnSimulate.Content = "Stop";
                    ChangeColorHelper(sender);
                    mRunningProfile.StartWriteToDB();
                }
                catch (Exception ex)
                {
                    mErrorMessage = ex.Message;
                }

            }
            else
            {
                mRunningProfile.Stop();
                btnSimulate.Content = "Start Sending";
                mRunningProfile = null;
                RevertColorHelper(sender);
            }

        }

        public void btnSimulateToJson_Click(object sender, RoutedEventArgs e)
        {
            SignalProfile sgProfile = new SignalProfile(mWave, mFreq, mAmpl, mRate);
            SimulationProfile smProfile = new SimulationProfile(sgProfile, mDuration);
            GenerateSignalData myProf = new GenerateSignalData(smProfile, true);

            myProf.ExportToJson(mSettingTab.mBeautifulJson);
            //MessageBox.Show("Finished Exporting", "Quick Infor", MessageBoxButton.OK, MessageBoxImage.Information);
            exportingIsFinished = true;
        }


        private void btnMSimuToDB_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("I'm in, Don't worry");
            if (_singleShotPressed)
            {
                mErrorMessage = "Single Shot is running!!";
                return;
            }
            if (mMultipleShotList.Count < 1)
                return;
            mErrorMessage = String.Empty;

            _multipleShotPressed = !_multipleShotPressed;

            if (_multipleShotPressed)
            {
                if (!mSettingTab.CheckConnection())
                {
                    mErrorMessage = "Connection Test is fail";
                    return;
                }
                mCurrentDatabase = mSettingTab.getCheckedDatabase();
                foreach (GenerateSignalData item in mMultipleShotList)
                {

                    if (item.checkSendToDB())
                    {
                        mRunningProfile = item;
                        mRunningProfile.setMyDB(mCurrentDatabase);
                        btnMSimuToDB.Content = "Stop";
                        ChangeColorHelper(sender);
                        mRunningProfile.StartWriteToDB();
                    }
                }
            }
            else
            {
                mRunningProfile.Stop();
                btnMSimuToDB.Content = "Send All to DB";
                RevertColorHelper(sender);
                mRunningProfile = null;

            }
        }
        private void btnMSimulateToJson_Click(object sender, RoutedEventArgs e)
        {
            if (mMultipleShotList.Count < 1)
                return;
            mErrorMessage = String.Empty;


            List<GenerateSignalData> sendList = new List<GenerateSignalData>();
            foreach (GenerateSignalData item in mMultipleShotList)
                if (item.checkSendToDB())
                {
                    item.GenerateData();
                    sendList.Add(item);
                }
            //string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string basePath = Directory.GetCurrentDirectory() + "\\AppData";
            Directory.CreateDirectory(basePath);
            string fileName = "MultipleSignal " + DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss ffff");
            string filePath = basePath + "\\" + fileName + ".json";

            JsonSerializer serializer;
            if (mSettingTab.mBeautifulJson)
                serializer = new JsonSerializer { Formatting = Formatting.Indented, TypeNameHandling = TypeNameHandling.Auto };
            else
                serializer = new JsonSerializer { TypeNameHandling = TypeNameHandling.Auto };
            using (StreamWriter writer = File.CreateText(filePath)) { serializer.Serialize(writer, sendList); }

        }


        public void MenuItemDel_Click(object sender, RoutedEventArgs e)
        {
            if (MultiShot.SelectedItem == null || mMultipleShotList.Count == 0) return;  //safety first
            try
            {
                mMultipleShotList.Remove((GenerateSignalData)MultiShot.SelectedItem);
                ItemDeleted = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("You must be so funny :)", "Wow", MessageBoxButton.OK, MessageBoxImage.Information);
                Debug.WriteLine(ex.Message);
            }

        }
        public void MenuItemAdd_Click(object sender, RoutedEventArgs e)
        {
            mMultipleShotList.Add(new GenerateSignalData());
            ItemAdded = true;
        }

        private Uri _uriImage;
        public Uri mUriImage
        {
            get { return _uriImage; }
            set
            {
                if (_uriImage != value)
                { _uriImage = value; OnPropertyChanged("mUriImage"); }
            }
        }

        private MyDBEntity mCurrentDatabase;

        private async void btnTestConn_Click(object sender, RoutedEventArgs e)
        {
            bool result = await Task.Run(() => mSettingTab.CheckConnection());
            if (result)
            {
                //mUriImage = new Uri("/Images/icons8-ok-48.png", UriKind.Relative);
                mUriImage = new Uri("pack://application:,,,/WpfApp2;component/Images/icons8-ok-48.png", UriKind.Absolute);
                mCurrentDatabase = mSettingTab.getCheckedDatabase();
            }
            else
                //mUriImage = new Uri(@"/Images/icons8-cancel-48.png", UriKind.Relative);
                mUriImage = new Uri("pack://application:,,,/WpfApp2;component/Images/icons8-cancel-48.png", UriKind.Absolute);
            //resultImg.DataContext = this;
            //resultImg.Source = new BitmapImage(mUriImage);
        }
        private void btnEnableEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!mSettingTab.ReverseEditMode())
                btnEnableEdit.Content = "Enable Edit";
            else btnEnableEdit.Content = "Disable Edit";
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            myTabControl.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            mSettingTab.PrintInfo();
            mSettingTab.SaveInfoToFile();
        }

        private void btnInsertProfile_Click(object sender, RoutedEventArgs e)
        {
            if (mSelectedTargetOnDB == null)
            {
                mErrorMessage = "Pls select Target first";
                return;
            }
            mErrorMessage = String.Empty;
            mMultipleShotList.Add(new GenerateSignalData(mWave, mFreq, mAmpl, mRate, mDuration, targetOnDb: mSelectedTargetOnDB));
        }
        private DBViewWindows mMyDBView;
        private GenerateSignalData _selectedRowOnDataGrid;
        public GenerateSignalData mSelectedRowOnDataGrid
        {
            get { return _selectedRowOnDataGrid; }
            set
            {
                if (_selectedRowOnDataGrid != value)
                { _selectedRowOnDataGrid = value; OnPropertyChanged("mSelectedRowOnDataGrid"); }
            }
        }
        //Under Testing
        private MySqlConnection conn;
        private void btnViewDatabase_Click(object sender, RoutedEventArgs e)
        {

            if (conn != null)
                conn.Close();

            string connStr = String.Format("server={0};user id={1}; password={2}; database={3}; pooling=false",
               mSettingTab.mServer, mSettingTab.mUserId, mSettingTab.mPassword, mSettingTab.mDatabaseName);
            string cmdStr = "select * from plc_data.plc_data order by id desc limit 100;";
            int windowsWidth = 1000;

            if (mSelectedRowOnDataGrid != null)
            {
                cmdStr = String.Format("select id, TimeStamp, {0} from plc_data.plc_data order by id desc limit 100;", mSelectedRowOnDataGrid.mTargetOnDB);
                windowsWidth = 450;
            }

            conn = new MySqlConnection(connStr);

            using (MySqlCommand cmd = new MySqlCommand(cmdStr, conn))
            {
                try
                {
                    conn.Open();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(cmd.ExecuteReader());
                    conn.Close();

                    mMyDBView = new DBViewWindows();
                    mMyDBView.myDBGrid.DataContext = dataTable;
                    mMyDBView.Width = windowsWidth;
                    mMyDBView.Show();

                    mMyDBView.Focus();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }


            }

            mSelectedRowOnDataGrid = null;

        }

        private void ChangeColorHelper(object sender)
        {
            mErrorMessage = "Running...";
            (sender as Button).Background = System.Windows.Media.Brushes.LightSalmon;
            (sender as Button).Foreground = System.Windows.Media.Brushes.White;
        }
        private void RevertColorHelper(object sender)
        {
            mErrorMessage = String.Empty;
            (sender as Button).Background = System.Windows.Media.Brushes.LightGreen;
            (sender as Button).Foreground = System.Windows.Media.Brushes.Black;
        }

        #endregion

        #region under Drafting
        //Not yet finished
        public void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            if ((int.TryParse(e.Text, out n)) == true)
            {
                mValidInput = true;
            }
            Regex regex = new Regex("[0-9.]+");
            if (!regex.IsMatch(e.Text))
            {
                mValidInput = false;
                MessageBox.Show("Number Accept Only !");
            }
        }
        //Not quite
        private void DoubleValidation(object sender, TextCompositionEventArgs e)
        {
            try
            {
                double.Parse(txtAmp.Text);
            }
            catch (Exception)
            {
                return;
            }
        }

    }

    #endregion


}
