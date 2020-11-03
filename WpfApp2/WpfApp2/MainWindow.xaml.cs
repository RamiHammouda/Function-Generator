using ScottPlot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2.Model;
using WpfApp2.ViewModel;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //SignalProfile testProfile;
            //testProfile = new SignalProfile(Model.WaveForm.Sine);
            //testProfile.PrintInfo();
            //testProfile = new SignalProfile((Model.WaveForm)1);
            //testProfile.PrintInfo();
            //SimulationProfile myProfile = new SimulationProfile((WaveForm)2, 1000, 2, 3, 12400);
            //myProfile.PrintInfo();
            //GenerateSignalData test = new GenerateSignalData(myProfile);
            //test.ExportToJson();
            GenerateSignalData newProfile = new GenerateSignalData(0,1,5,20,1);
            newProfile.ExportToJson();
            //newProfile.PrintData();

            TestFunc();
        }

        public void TestFunc()
        {

            int pointCount = 50;
            double[] x = DataGen.Consecutive(pointCount);
            double[] sin = DataGen.Sin(pointCount);
            double[] cos = DataGen.Cos(pointCount);

            myWpfPlot.plt.PlotScatter(x, sin);
            myWpfPlot.plt.PlotScatter(x, cos);
            myWpfPlot.plt.Style(ScottPlot.Style.Light2);
            myWpfPlot.Render();

            //plt.SaveFig("PlotTypes_Scatter_CustomizeLines.png");
        }
        public long OffsetFreq, Freq,TickRate,Rate;
        public double OffsetAmpl, Ampl;
        public decimal Tick,Duration;

        public void initiateWithGUI()
        {

            //Freq = Convert.ToInt64(txtFreq.Text);
            //Ampl = Convert.ToDouble(txtAmp.Text);
        }

        private void sldFreq_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //Slider slider = sender as Slider;
            //Console.WriteLine((sender as Slider).Value);
            OffsetFreq = (long)(sender as Slider).Value;
        }

        private void sldAmp_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            OffsetAmpl = (sender as Slider).Value;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            TickRate = (long)Math.Round(1e7m / Rate,0);
            Console.WriteLine("Tick " + TickRate);
            //if (rdoSine.IsChecked == true)
            if (true)
            {
                LinkedList<long> TimeStampList = new LinkedList<long>();
                LinkedList<double> AmplList = new LinkedList<double>();
                DateTime start = DateTime.Now;
                Console.WriteLine(TimeStampList.ToString());
                Console.WriteLine(AmplList.ToString());

                StringBuilder sb = new StringBuilder();
                for (long i = 0; i < Rate * Duration; i++)
                {
                    sb.AppendLine(String.Format("{0} {1} {2}",i,start.Ticks + i * TickRate,GetAmplSine(i*TickRate)));
                    
                };

                //string path = @"C:\Users\Admin\Desktop\Testwriting.txt";
                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Testwriting.txt";
              
                File.AppendAllText(filePath, sb.ToString());
            }
        }

        public double GetAmplSine(long attime)
        {
            //Console.WriteLine("TEst");
            //Console.WriteLine(Freq);
            Console.WriteLine(Ampl*Math.Sin(2 * Math.PI*Freq*0.25));
            return Ampl * Math.Sin(2*Math.PI*Freq*attime/1e7);
        }

        public double GetValueSawtooth(long attime)
        {
            return 0;
        }

    }
}
