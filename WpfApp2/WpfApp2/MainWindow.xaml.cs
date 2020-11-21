using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WpfApp2.Model;
using WpfApp2.ViewModel;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        
        public MainWindow()
        {           
            InitializeComponent();


            this.DataContext = this;

            initiateDefaultValue();

        }

        private void VisualizateData(SimulationProfile smProfile, int numberOfWave)
        {
            double displayduration = (double)numberOfWave / smProfile.getFreq() + (double)1 / smProfile.getRate();
            GenerateSignalData displayData = new GenerateSignalData(smProfile.getSignalProfile(), displayduration, true);
            //displayData.PrintData();
            double linewidth = 1, marksize = 5;
            string text = $"Wave: {displayData.getWave()}\nFreq: {displayData.getFreq()} Hz\nAmpl: {displayData.getAmpl()} V\nRate: {displayData.getRate()}\nDura: {smProfile.getDuration()} s";
            double[] x = GenerateSignalData.ConvertToDouble(displayData.getNo());
            double[] y = displayData.getAmplData();
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
            myWpfPlot.Render();
        }

        #region Propertise

        public bool mValidInput { get; set; }
        public long mRate, mDuration;
        public double mOffsetAmpl;
        public long mOffsetFreq { get; set; }

        //public ListBindableAttribute targetFromDB = new ListBindableAttribute();

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

        private long _freq;
        public long mFreq
        {
            get { return _freq; }
            set
            {
                if (_freq != value)
                { _freq = value; OnPropertyChanged("mFreq"); }
            }
        }
        private double _ampl;
        public double mAmpl
        {
            get { return _ampl; }
            set
            {
                if (_ampl != value)
                { _ampl = value; OnPropertyChanged("mAmpl"); }
            }
        }
        #endregion

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

        public void initiateDefaultValue()
        {
            mFreq = 25;
            mAmpl = 5;
            mRate = 1000;
            mDuration = 10;
            mWave = WaveForm.Sine;
            mValidInput = false;

            this.PropertyChanged += AutoDrawing;
            OnPropertyChanged("IamIronMan");

            _multipleShotList = new ObservableCollection<GenerateSignalData>()
            { new GenerateSignalData(),
             new GenerateSignalData(0,230,7,510,2,sendToDb:true),
              new GenerateSignalData(WaveForm.Random,320,3,993,2,targetOnDb:"Inputs_Entschlammung1_Status"),
              new GenerateSignalData(WaveForm.Sawtooth,sendToDb:true,targetOnDb:"Inputs_TestVarLReal")
            };

        }

        #region Some Control Element
        private void sldFreq_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mOffsetFreq = (long)(sender as Slider).Value;
        }

        private void sldAmp_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mOffsetAmpl = Math.Round((sender as Slider).Value / 1000, 3);
        }

        private void btnFreqPlus_Click(object sender, RoutedEventArgs e)
        {
            mFreq += mOffsetFreq;
        }

        private void btnFreqMinus_Click(object sender, RoutedEventArgs e)
        {
            mFreq -= mOffsetFreq;
        }

        private void btnAmplPlus_Click(object sender, RoutedEventArgs e)
        {
            mAmpl += Math.Round(mOffsetAmpl, 3);
        }
        private void btnAmplMinus_Click(object sender, RoutedEventArgs e)
        {
            mAmpl -= Math.Round(mOffsetAmpl, 3);
        }

        #endregion
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            initiateDefaultValue();
        }

        private void AutoDrawing(object sender, EventArgs e)
        {
            SimulationProfile currentSignal = new SimulationProfile(mWave, mFreq, mAmpl, mRate, mDuration);
            VisualizateData(currentSignal, 4);
        }

        private void btnSimulate_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Just Kidding", "Quick Infor", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnSimulateToJson_Click(object sender, RoutedEventArgs e)
        {
            SignalProfile sgProfile = new SignalProfile(mWave, mFreq, mAmpl, mRate);
            SimulationProfile smProfile = new SimulationProfile(sgProfile, mDuration);
            GenerateSignalData myProf = new GenerateSignalData(smProfile, true);

            myProf.ExportToJson();
            MessageBox.Show("Finished Exporting", "Quick Infor", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnMSimulateToJson_Click(object sender, RoutedEventArgs e)
        {
            if (mMultipleShotList.Count < 1)
                return;
            List<GenerateSignalData> sendList = new List<GenerateSignalData>();
            foreach (GenerateSignalData item in mMultipleShotList)
                if (item.checkSendToDB() == true) { 
                    item.GenerateData();
                    sendList.Add(item);
                }
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //string fileName = DateTime.Now.Ticks.ToString();
            string fileName = "PrintList";
            //string filePath = desktopPath + @"\GenerateData.json";
            string filePath = desktopPath + "\\" + fileName + ".json";

            var serializer = new JsonSerializer { Formatting = Formatting.Indented, TypeNameHandling = TypeNameHandling.Auto };
            //var serializer = new JsonSerializer { TypeNameHandling = TypeNameHandling.Auto };
            using (StreamWriter writer = File.CreateText(filePath)) { serializer.Serialize(writer, sendList); }

        
        //myProf.ExportToJson();
        MessageBox.Show("Finished Exporting", "Quick Infor", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void MenuItemDel_Click(object sender, RoutedEventArgs e)
        {
            if (MultiShot.SelectedItem == null || mMultipleShotList.Count == 0) return;  //safety first

            try
            {
                mMultipleShotList.Remove((GenerateSignalData)MultiShot.SelectedItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show("You must be so funny :)", "Wow", MessageBoxButton.OK, MessageBoxImage.Information);
                Debug.WriteLine(ex.Message);
            }
        }
        private void MenuItemAdd_Click(object sender, RoutedEventArgs e)
        {
            mMultipleShotList.Add(new GenerateSignalData());
        }

        //Not quite finish
        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            //Regex regex = new Regex("[^a-zA-Z]+");

            Regex regex = new Regex("[0-9]+");
            if (!regex.IsMatch(e.Text))
            {
                mValidInput = false;
                MessageBox.Show("Number Accept Only !");
            }

        }
        //Not quite
        private void DoubleValidation(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex("[0-9.]+");
            if (!regex.IsMatch(e.Text))
            {
                mValidInput = false;
                MessageBox.Show("Number Accept Only !");
            }
            try
            {
                double.Parse(txtAmp.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Pls input a double number !");
                return;
            }
        }
    }

    public class ComparisonConverter : IValueConverter
    {
        //public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        //{
        //    return value?.Equals(parameter);
        //}

        //public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        //{
        //    return value?.Equals(true) == true ? parameter : Binding.DoNothing;
        //}

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((WaveForm)value).HasFlag((WaveForm)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }

}
