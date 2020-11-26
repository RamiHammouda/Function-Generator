using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace WpfApp2
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    class AmplitudeTesting
    {
        [Test]
        public static void CanInputAmplitude_InputIsCorrect_ReturnsTrue()
        {
            //Arrange
            var testingWindow = new MainWindow();
            //Act
            testingWindow.txtAmp.Text = Convert.ToString(50);
            //Assert
            Assert.AreEqual(true, testingWindow.mValidInput);
        }
        [Test]
        public static void CanotInputNegativeAmplitude_InputIsOutOfBoundaries_ReturnsFalse()
        {
            //Arrange
            var testingWindow = new MainWindow();
            //Act
            testingWindow.txtAmp.Text = Convert.ToString(-50);
            //Assert
            Assert.AreEqual(false, testingWindow.mValidInput);
        }
        [Test]
        public static void CannotOtherTypeThenDoubleAmplitude_InputTypeIsWrong_ReturnsFalse()
        {
            //Arrange
            var testingWindow = new MainWindow();
            //Act
            testingWindow.txtAmp.Text = Convert.ToString("a");
            //Assert
            Assert.AreEqual(false, testingWindow.mValidInput);
        }
        [Test]
        public static void CanIncreaseAmplitudeyWithOffset_PlusButtonPressed_ReturnsBiggerValue()
        {
            //Arrange
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            //Act
            testingWindow.btnAmplPlus_Click(sender,e);
            //Assert
            Assert.AreEqual(true, testingWindow.offsetAmpValueIncreased);
        }
        [Test]
        public static void CanDecreaseFrequencyWithOffset_MinusButtonPressed_ReturnsLowerValue()
        {
            //Arrange
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            //Act
            testingWindow.btnAmplMinus_Click(sender, e);
            //Assert
            Assert.AreEqual(true, testingWindow.offsetAmpValueDecreased);
        }
    }
}
