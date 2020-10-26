using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
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

            TestFunc();
        }

        public void TestFunc()
        {

            int pointCount = 51;
            double[] x = DataGen.Consecutive(pointCount);
            double[] sin = DataGen.Sin(pointCount);
            double[] cos = DataGen.Cos(pointCount);

            myWpfPlot.plt.PlotScatter(x, sin);
            myWpfPlot.plt.PlotScatter(x, cos);
            myWpfPlot.plt.Style(ScottPlot.Style.Light2);
            myWpfPlot.Render();

            //plt.SaveFig("PlotTypes_Scatter_CustomizeLines.png");

        }

        private void sldFreq_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void sldAmp_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
