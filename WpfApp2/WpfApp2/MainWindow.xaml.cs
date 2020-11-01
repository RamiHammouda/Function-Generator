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

            //Temporary

            Freq = 1;
            Ampl = 5;
            Duration = 1m;
            Rate = 9600;
            TestFunc();
            Console.WriteLine(637397472332016471 + 9995906);
            Console.WriteLine($"Freq:{Freq} Ampl:{Ampl} Duration: {Duration} Rate: {Rate}");

            Console.WriteLine($"GetAmplSine {GetAmplSine(2500000)}");
            
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
                //Should combine Wavefrom in Datagenerate?
                //Sine mySine = new Sine(Freq, Ampl, 9600);
                //DataGenerate aData = new DataGenerate(mySine,10);
                LinkedList<long> TimeStampList = new LinkedList<long>();
                LinkedList<double> AmplList = new LinkedList<double>();
                DateTime start = DateTime.Now;
                Console.WriteLine(TimeStampList.ToString());
                Console.WriteLine(AmplList.ToString());

                StringBuilder sb = new StringBuilder();
                for (long i = 0; i < Rate * Duration; i++)
                {
                    //TimeStampList.AddFirst(i*Tick);
                    //AmplList.AddFirst(GetAmplSine(i));
                    //Console.WriteLine(start.Ticks);
                    //Console.WriteLine(Math.Round(i * Tick,0));
                    //sb.AppendLine(Convert.ToString(start.Ticks + i*Tick));
                    sb.AppendLine(String.Format("{0} {1} {2}",i,start.Ticks + i * TickRate,GetAmplSine(i*TickRate)));
                    
                };
                //Console.WriteLine(sb);
                //Console.WriteLine(TimeStampList.First);
                //Console.WriteLine(AmplList.First);

                //string path = @"C:\Users\Admin\Desktop\Testwriting.txt";
                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Testwriting.txt";

              
                File.AppendAllText(filePath, sb.ToString());

            }
        }

        public double GetAmplSine(long attime)
        {
            
            //Console.WriteLine("TEst");
            //Console.WriteLine(Freq);
            //Console.WriteLine(Ampl*Math.Sin(2 * Math.PI*Freq*0.25));
            return Ampl * Math.Sin(2*Math.PI*Freq*attime/1e7);
        }

        public double GetValueSawtooth(long attime)
        {
            return 0;
        }

    }
}
