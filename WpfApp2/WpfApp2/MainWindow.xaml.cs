using Newtonsoft.Json;
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
using System.Threading;
using System.Globalization;


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
        public bool itemAdded = false;
        public bool itemDeleted = false;
        public bool validInput = false;
        public bool cannotInsertEmptyProfile = false;
        public bool addedToMultipleShotList = false;
        public bool notEnouhItems = false;
        public bool itemsAreSaved = false;
        public bool connectionToDBError = false;
        public bool itemsAreSavedToDB = false;
        public bool dataBaseWindowOpened = false;
        public bool editIsEnabled = false;
        public bool connectionTested = false;
        public bool infosSaved = false;
        public int n;
        public bool mValidInput { get; set; }
        #endregion

        #region Properties

        private string _errorMessage;
        public string mErrorMessage { get { return _errorMessage; } set { if (_errorMessage != value) { _errorMessage = value; OnPropertyChanged("mErrorMessage"); } } }
        private double _freq, _rate, _ampl;
        [Range(0.00001, 4, ErrorMessage = "Sending Data Rate to Database is limitted to maximum 4 : 4 times/s")]
        public double mRate { get { return _rate; } set { if (_rate != value) { _rate = value; OnPropertyChanged("mRate"); } } }
        public long mDuration;

        private double mOffsetFreq, _offsetAmpl, _maxOffsetFreq, _defaultMaxOffsetFreq, _tickFreqOnSlider, _offSetAmplFromInput;
        public double mOffsetAmpl { get { return _offsetAmpl; } set { if (_offsetAmpl != value) { _offsetAmpl = value; OnPropertyChanged("mOffsetAmpl"); } } }
        public double mMaxOffsetFreq { get { return _maxOffsetFreq; } set { if (_maxOffsetFreq != value) { _maxOffsetFreq = value; OnPropertyChanged("mMaxOffsetFreq"); } } }
        public double mTickFrequencyOnSlider { get { return _tickFreqOnSlider; } set { if (_tickFreqOnSlider != value) { _tickFreqOnSlider = value; OnPropertyChanged("mTickFrequencyOnSlider"); } } }
        public double mOffsetAmptFromInput { get { return _offSetAmplFromInput; } set { if (_offSetAmplFromInput != value) { _offSetAmplFromInput = value; OnPropertyChanged("mOffsetAmptFromInput"); } } }

        private WaveForm _wave;
        public WaveForm mWave { get { return _wave; } set { if (_wave != value) { _wave = value; OnPropertyChanged("mWave"); } } }

        [Range(0.0001, 4, ErrorMessage = "Frequency must from {1} to {2}")]
        [Required(ErrorMessage = "Frequency is required")]
        public double mFreq { get { return _freq; } set { if (_freq != value) { _freq = value; OnPropertyChanged("mFreq"); myCheckBoxEvent.Invoke(this, new PropertyChangedEventArgs("egal")); } } }
        //[Range(0.001, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        [Range(0.001, Double.PositiveInfinity, ErrorMessage = "Amplitude must from {1}")]
        [Required(ErrorMessage = "Amplitude is required")]
        public double mAmpl { get { return _ampl; } set { if (_ampl != value) { _ampl = value; OnPropertyChanged("mAmpl"); } } }

        private bool _chkBoxSynFreq;
        public bool mChkBoxSynFreq { get { return _chkBoxSynFreq; } set { if (_chkBoxSynFreq != value) { _chkBoxSynFreq = value; OnPropertyChanged("mChkBoxSynFreq"); myCheckBoxEvent.Invoke(this, new PropertyChangedEventArgs("egal")); } } }

        public SettingInfor mSettingTab { get; set; }

        public event PropertyChangedEventHandler PropertyChanged, myCheckBoxEvent;


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnMyCheckboxChange(object sender, EventArgs e)
        {
            if (mChkBoxSynFreq)
                mMaxOffsetFreq = mFreq;
            else
                mMaxOffsetFreq = _defaultMaxOffsetFreq;
            mTickFrequencyOnSlider = mMaxOffsetFreq / 20;
        }

        public ObservableCollection<GenerateSignalData> _multipleShotList;
        public ObservableCollection<GenerateSignalData> mMultipleShotList { get { return _multipleShotList; } set { if (_multipleShotList != value) { _multipleShotList = value; OnPropertyChanged("_multipleShotList"); } } }
        private List<string> _mytargetOnDB;
        public List<string> mMyTargetOnDB { get { return _mytargetOnDB; } set { if (_mytargetOnDB != value) { _mytargetOnDB = value; OnPropertyChanged("mMyTargetOnDB"); } } }

        private string _selectedTargetOnDB;
        public string mSelectedTargetOnDB { get { return _selectedTargetOnDB; } set { if (_selectedTargetOnDB != value) { _selectedTargetOnDB = value; OnPropertyChanged("mSelectedTargetOnDB"); } } }

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
            //mOffsetFreq = Math.Round((sender as Slider).Value / 1000, 3);
            mOffsetFreq = Math.Round((sender as Slider).Value, 3);
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


        public List<string> mTriggerTypeList { get; set; }
        private string _mSelectedTrigger;
        public string mSelectedTrigger { get { return _mSelectedTrigger; } set { if (_mSelectedTrigger != value) { _mSelectedTrigger = value; OnPropertyChanged("mSelectedTrigger"); TriggerChangeHandler(); } } }

        private DateTime _mChoosenTime;
        public DateTime mChoosenTime { get { return _mChoosenTime; } set { if (_mChoosenTime != value && DateTime.Now <= value ) { _mChoosenTime = value; OnPropertyChanged("mChoosenTime"); Console.WriteLine(mChoosenTime); SetUpTimer(mChoosenTime); } } }

        #endregion

        #region Functions and Buttons
        /// <summary>
        /// Reset all parameters of first tab to default
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnReset_Click(object sender, RoutedEventArgs e)
        {
            initiateDefaultValue();
        }

        /// <summary>
        /// Create some default value, or initiate some value, some function, load setting, read information from file...
        /// </summary>
        public void initiateDefaultValue()
        {
            this.DataContext = this;

            this.myCheckBoxEvent += OnMyCheckboxChange;

            mFreq = 0.1;
            mAmpl = 5;
            mRate = 2;
            mDuration = 0;
            mWave = WaveForm.Sine;

            mValidInput = false;
            //sldFreq.DataContext = this;


            _defaultMaxOffsetFreq = 0.2;
            mMaxOffsetFreq = _defaultMaxOffsetFreq;
            mMaxOffsetFreq = 0.2;
            mTickFrequencyOnSlider = mMaxOffsetFreq / 20;


            mCurrentProfile = new GenerateSignalData(mWave, mFreq, mAmpl, mRate, mDuration);
            this.PropertyChanged += AutoDrawing;

            OnPropertyChanged("IamIronMan");

            if (File.Exists(GenerateSignalData.mFilePath))
                _multipleShotList = new ObservableCollection<GenerateSignalData>(GenerateSignalData.ImportProfileFromJson());
            else
            {
                _multipleShotList = new ObservableCollection<GenerateSignalData>()
                {
                    //new GenerateSignalData(),
                    //new GenerateSignalData(0, 2, 7, 1, 0, sendToDb: true),
                    //new GenerateSignalData(WaveForm.Random, targetOnDb: "Inputs_Entschlammung1_Status"),
                    //new GenerateSignalData(WaveForm.Sawtooth, sendToDb: true, targetOnDb: "Inputs_TestVarReal")
                };
            }


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
                try
                {
                    mMyTargetOnDB = mSettingTab.LoadFinalTargetList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            mRate = mSettingTab.mRate;

            sldFreq.Value = 0;
            sldAmp.Value = 0;

            //only for Image Connection Result
            resultImg.DataContext = this;


            mTriggerTypeList = new List<string> { "Default", "TimeTriger", "Random" };
            mSelectedTrigger = mTriggerTypeList[0];
            TriggerChangeHandler();
            mChoosenTime = DateTime.Now;

        }
        private List<ColumnDBSelectableHelper> _offLineTargetList { get { List<ColumnDBSelectableHelper> aList = new List<ColumnDBSelectableHelper>() { new ColumnDBSelectableHelper(true, "Offline"), new ColumnDBSelectableHelper(true, "Just Kidding") }; return aList; } }
        private List<string> _offLineList { get { List<string> aList = new List<string>() { "Offline", "Offline only" }; return aList; } }
        /// <summary>
        /// Visualizate the current parameter to wave form
        /// </summary>
        /// <param name="smProfile"></param>
        /// <param name="numberOfWave"></param>
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
            myWpfPlot.plt.PlotScatter(x, y, color: Color.Blue, lineWidth: linewidth, markerSize: marksize, label: text);
            myWpfPlot.plt.Style(ScottPlot.Style.Light2);
            myWpfPlot.plt.Title("Signal Data", fontName: "Verdana", color: Color.BlueViolet, bold: true);
            myWpfPlot.plt.YLabel("Amplitude", fontSize: 14, color: Color.Green, bold: true);
            myWpfPlot.plt.XLabel("Time(Ticks)", color: Color.Green, bold: true, fontSize: 14);
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
        private GenerateSignalData mCurrentProfile;

        /// <summary>
        /// This method will be called whenever any parameter changed, It create profile to simulate and call <see cref="VisualizateData"/> funtion to draw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoDrawing(object sender, EventArgs e)
        {
            mCurrentProfile = new GenerateSignalData(mWave, mFreq, mAmpl, mRate, mDuration);
            SimulationProfile currentSignal = mCurrentProfile.getSimulationProfile();
            VisualizateData(currentSignal, 4);
        }

        private bool _singleShotPressed, _multipleShotPressed;
        /// <summary>
        /// Insert data in only one column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSimulate_Click(object sender, RoutedEventArgs e)
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

            _singleShotPressed = !_singleShotPressed;
            if (_singleShotPressed)
            {
                if (!mSettingTab.CheckConnection())
                {
                    mErrorMessage = "Connection Test is fail";
                    return;
                }
                ChangeColorHelper(sender);

                mCurrentProfile.setTargetOnDB(mSelectedTargetOnDB);

                //-----------------------------------
                mCurrentDatabase = new MyDBEntity(mSettingTab);

                // New Dictionary for new values
                myDataDict = new Dictionary<string, string>();

                // Get Dictionary From DB Class with all Columnnames and the last line
                myDataDict = mCurrentDatabase.GetData();

                //To check result
                Console.WriteLine("Before:::::::::::::::");
                foreach (KeyValuePair<string, string> kvp in myDataDict)
                {
                    Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                }

                Console.WriteLine("Starting");
                //2- Start create data and insert to Dictionary then send to DB this dictionnay
                long now;

                while (!_Stop)
                {
                    now = DateTime.Now.Ticks;

                    myDataDict[mCurrentProfile.getTargetOnDB()] = Convert.ToString(mCurrentProfile.getWaveValue(now), CultureInfo.InvariantCulture);
                    Thread.Sleep((int)(1000 / mRate));

                    Console.WriteLine("After:::::::::::::::");
                    foreach (KeyValuePair<string, string> kvp in myDataDict)
                    {
                        Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                    }

                    await Task.Run(() =>
                    {
                        mCurrentDatabase.InsertRow(myDataDict);
                    });
                }

                _Stop = false;
                Console.WriteLine("Finish Writing");
            }
            else
            {
                _Stop = true;
                RevertColorHelper(sender);
            }

        }
        /// <summary>
        /// Obsolete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnSimulateToJson_Click(object sender, RoutedEventArgs e)
        {
            SignalProfile sgProfile = new SignalProfile(mWave, mFreq, mAmpl, mRate);
            SimulationProfile smProfile = new SimulationProfile(sgProfile, mDuration);
            GenerateSignalData myProf = new GenerateSignalData(smProfile, true);

            myProf.ExportToJson(mSettingTab.mBeautifulJson);
            exportingIsFinished = true;
        }

        /// <summary>
        /// Save all Profiles in list to Json file in current exe Directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnSaveProfilesToJson_Click(object sender, RoutedEventArgs e)
        {
            GenerateSignalData.ExportProfilesToJson(_multipleShotList.ToList());
        }

        private Dictionary<string, string> myDataDict;
        private bool _Stop = false;

        private Timer timer;
        /// <summary>
        /// Receive a DateTime to set Timer and trigger sending data to database at this specific time.
        /// If randomTrigger = true, then it will be recursive Method:
        /// It calls itself to trigger again at random time at day, so that it will fully automatically start / stop sending data.
        /// If nexttime trigger smaller than 10 Minutes then this recursive will stop as well as the sending data button;
        /// If TriggerType CommboBox is changed during running Timer, then current timer will be detroyed;
        /// </summary>
        /// <param name="alertTime"></param>
        private void SetUpTimer(DateTime alertTime,bool randomTrigger=false)
        {
            TimeSpan timeToGo = alertTime - DateTime.Now;

            if (randomTrigger)
            {
                if (timeToGo < TimeSpan.FromMinutes(10)) { _Stop = true; return; }

            }
            if (timeToGo < TimeSpan.Zero) return;
            mErrorMessage = $"Sending data will be triggered at {alertTime}";

            timer = null;
            this.timer = new Timer(x =>
            {
                if(timer!=null) TriggerButton();
                if (randomTrigger) SetUpTimer(PickARandomTimeAtDay(),true);
            }, null, timeToGo, Timeout.InfiniteTimeSpan);

        }
        /// <summary>
        /// Just pick a random time at day
        /// </summary>
        /// <returns>DateTime value</returns>
        private DateTime PickARandomTimeAtDay()
        {
            DateTime endToday, current = DateTime.Now;
            Random random = new Random();
            endToday = new DateTime(current.Year, current.Month, current.Day, 23, 59, 59);
            return current + TimeSpan.FromMinutes(random.Next(0, (int)(endToday - current).TotalMinutes));
        }
        /// <summary>
        /// Handler the change in TriggerType List to corresponding action
        /// </summary>
        private void TriggerChangeHandler()
        {
            Console.WriteLine(mMultipleShotList);
            mErrorMessage = String.Empty;
            timer = null;
            if (mSelectedTrigger == mTriggerTypeList[1]) //TimeTrigger
            {
                if (mMultipleShotList.Count<1) { mErrorMessage = "Pls create simulation profiles first"; return; }
                lblTimeTrigger.Visibility = Visibility.Visible;
                timePicker.Visibility = Visibility.Visible;
                mChoosenTime = DateTime.Now;
                return;
            }

            if (mSelectedTrigger == mTriggerTypeList[2]) //Random
            {
                if (mMultipleShotList.Count < 1) { mErrorMessage = "Pls create simulation profiles first"; return; }
                lblTimeTrigger.Visibility = Visibility.Hidden;
                timePicker.Visibility = Visibility.Hidden;
                DateTime rdTime = PickARandomTimeAtDay();
                Console.WriteLine(rdTime);
                SetUpTimer(rdTime,true);
                return;
            }

            lblTimeTrigger.Visibility = Visibility.Hidden;
            timePicker.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Trigger a button in UI Thread
        /// </summary>
        private void TriggerButton()
        {
            this.Dispatcher.Invoke((Action)(()=>
            {//this refer to form in WPF application
                btnMSimuToDB.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }));
        }

        /// <summary>
        /// Add multiple values to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void btnMSimuToDB_Click(object sender, RoutedEventArgs e)
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
            mSelectedRowOnDataGrid = null;
            mSelectedTargetOnDB = null;
            _multipleShotPressed = !_multipleShotPressed;

            if (_multipleShotPressed)
            {
                if (!mSettingTab.CheckConnection())
                {
                    mErrorMessage = "Connection Test is fail";
                    return;
                }
                ChangeColorHelper(sender);
                //-----------------------------
                mCurrentDatabase = new MyDBEntity(mSettingTab);

                // New Dictionary for new values
                myDataDict = new Dictionary<string, string>();

                // Get Dictionary From DB Class with all Columnnames and the last line
                myDataDict = mCurrentDatabase.GetData();
                Console.WriteLine("Finished Getting");

                //To check result
                Console.WriteLine("Before:::::::::::::::");
                foreach (KeyValuePair<string, string> kvp in myDataDict)
                {
                    Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                }

                Console.WriteLine("Starting");

                //2- Start create data and insert to Dictionary then send to DB this dictionnay

                long now;

                while (!_Stop)
                {
                    now = DateTime.Now.Ticks;
                    foreach (GenerateSignalData item in mMultipleShotList)
                    {
                        if (item.checkSendToDB())
                        {
                            myDataDict[item.getTargetOnDB()] = Convert.ToString(item.getWaveValue(now), CultureInfo.InvariantCulture);
                        }
                    }

                    Thread.Sleep((int)(1000 / mRate));
                    //Showing only
                    Console.WriteLine("After:::::::::::::::");
                    foreach (KeyValuePair<string, string> kvp in myDataDict)
                    {
                        Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                    }

                    await Task.Run(() =>
                    {
                        mCurrentDatabase.InsertRow(myDataDict);
                    });
                }

                Console.WriteLine("Finished Writing");
                _Stop = false;
            }
            else
            {
                _Stop = true;
                RevertColorHelper(sender);
            }
        }

        /// <summary>
        /// Obsolete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnMSimulateToJson_Click(object sender, RoutedEventArgs e)
        {
            if (mMultipleShotList.Count < 1)
                return;
            if (mMultipleShotList.Count < 1)
            {
                notEnouhItems = true;
            }
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
            itemsAreSaved = true;

        }
        /// <summary>
        /// Delete 1 item/row in datagrid in context menu in right click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MenuItemDel_Click(object sender, RoutedEventArgs e)
        {
            if (MultiShot.SelectedItem == null || mMultipleShotList.Count == 0) return;  //safety first
            try
            {
                mMultipleShotList.Remove((GenerateSignalData)MultiShot.SelectedItem);
                itemDeleted = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("You must be so funny :)", "Wow", MessageBoxButton.OK, MessageBoxImage.Information);
                Debug.WriteLine(ex.Message);
            }

        }
        /// <summary>
        /// Add 1 item/row in datagrid in context menu in right click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MenuItemAdd_Click(object sender, RoutedEventArgs e)
        {
            mMultipleShotList.Add(new GenerateSignalData());
            itemAdded = true;
        }

        private Uri _uriImage;
        public Uri mUriImage { get { return _uriImage; } set { if (_uriImage != value) { _uriImage = value; OnPropertyChanged("mUriImage"); } } }

        private MyDBEntity mCurrentDatabase;

        /// <summary>
        /// Test Connection method in Setting Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void btnTestConn_Click(object sender, RoutedEventArgs e)
        {
            connectionTested = true;
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
        }
        /// <summary>
        /// Enable/disable method in setting tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnEnableEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!mSettingTab.ReverseEditMode())
                btnEnableEdit.Content = "Enable Edit";
            else btnEnableEdit.Content = "Disable Edit";
            editIsEnabled = true;
        }

        /// <summary>
        /// cancel button in setting tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            myTabControl.SelectedIndex = 0;
        }
        /// <summary>
        /// save information in settingtab to json file in current exe folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnSave_Click(object sender, RoutedEventArgs e)
        {
            mSettingTab.PrintInfo();
            mSettingTab.SaveInfoToFile();
            infosSaved = true;
        }
        /// <summary>
        /// Move current profile with chosen parameter to Datagrid in Multiple Shot Tab (2.Tab)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnInsertProfile_Click(object sender, RoutedEventArgs e)
        {
            if (mSelectedTargetOnDB == null)
            {
                mErrorMessage = "Pls select Target first";
                cannotInsertEmptyProfile = true;
                return;
            }
            mErrorMessage = String.Empty;
            mMultipleShotList.Add(new GenerateSignalData(mWave, mFreq, mAmpl, mRate, mDuration, targetOnDb: mSelectedTargetOnDB));
            addedToMultipleShotList = true;
        }
        private DBViewWindows mMyDBView;
        private GenerateSignalData _selectedRowOnDataGrid;
        public GenerateSignalData mSelectedRowOnDataGrid { get { return _selectedRowOnDataGrid; } set { if (_selectedRowOnDataGrid != value) { _selectedRowOnDataGrid = value; OnPropertyChanged("mSelectedRowOnDataGrid"); } } }
        //Under Testing
        private MySqlConnection conn;
        /// <summary>
        /// Method to view current connected database based on picking column.
        /// Click none then show all columns on database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnViewDatabase_Click(object sender, RoutedEventArgs e)
        {

            if (conn != null)
                conn.Close();

            string connStr = String.Format("server={0};user id={1}; password={2}; database={3}; pooling=false",
               mSettingTab.mServer, mSettingTab.mUserId, mSettingTab.mPassword, mSettingTab.mDatabaseName);
            string cmdStr = $"select * from {mSettingTab.mDatabaseName}.{mSettingTab.mTabName} order by id desc limit 200;";
            string showText = "Lastest 200 value on database";
            int windowsWidth = 1000;

            if (mSelectedRowOnDataGrid != null)
            {
                cmdStr = String.Format("select id, TimeStamp, {0} from {1}.{2} where {0} is not null order by id desc limit 100;", mSelectedRowOnDataGrid.mTargetOnDB, mSettingTab.mDatabaseName, mSettingTab.mTabName);
                windowsWidth = 450;
                showText = "Lastest 100 value without Null on database";

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
                    mMyDBView.txtDbViewWindows.Text = showText;
                    mMyDBView.Show();

                    mMyDBView.Focus();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            dataBaseWindowOpened = true;

            mSelectedRowOnDataGrid = null;
        }
        /// <summary>
        /// Change color, label of current button to running state
        /// </summary>
        /// <param name="sender"></param>
        private void ChangeColorHelper(object sender)
        {
            mErrorMessage = "Running...";
            Button thisButton = sender as Button;
            thisButton.Content = "Stop";
            thisButton.Background = System.Windows.Media.Brushes.LightSalmon;
            thisButton.Foreground = System.Windows.Media.Brushes.White;
        }
        /// <summary>
        /// Change back color, label of current button to stop/waiting state
        /// </summary>
        /// <param name="sender"></param>
        private void RevertColorHelper(object sender)
        {
            mErrorMessage = String.Empty;
            Button thisButton = sender as Button;
            thisButton.Content = "Start Sending";
            System.Windows.Media.BrushConverter bc = new System.Windows.Media.BrushConverter();
            thisButton.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#b2fcff");
            thisButton.Foreground = System.Windows.Media.Brushes.Black;
        }

        #endregion

        #region under Drafting
        //Obsolete
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
        //Only for testing
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

        #endregion

    }

}
