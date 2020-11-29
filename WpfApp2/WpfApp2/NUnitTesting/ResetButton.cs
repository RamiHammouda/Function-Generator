using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace WpfApp2.SingleShot
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    class ResetButton
    {
        //[Test]
        public static void CanResetAllValuesToDefault_ResetIsPressed_ReturnsDefaultValues()
        {
            //Arrange
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            //Act
            testingWindow.btnReset_Click(sender,e);
            //Assert
            Assert.AreEqual(true, ((testingWindow.mFreq == 25) && (testingWindow.mAmpl == 5) && (testingWindow.mRate == 1000) && (testingWindow.mDuration == 10) && (testingWindow.mWave == Model.WaveForm.Sine)));
        }

    }
}
