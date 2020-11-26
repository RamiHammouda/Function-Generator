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
    class AmplitudeTesting
    {
        //TestName_Scenario_ExpectedBehavior
        /// <AmplitudeInputTest>
        /// A new application window will be created. The amplitude input box will get the value of "50" then it will be verified if this value will be accepted as a valid input.
        /// </AmplitudeInputTest>
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
        /// <AmplitudeInputTest>
        /// A new application window will be created. The amplitude input box will get the value of "-50" then it will be verified if this value will be rejected as it should.
        /// </AmplitudeInputTest>
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
        /// <AmplitudeInputTest>
        /// A new application window will be created. The amplitude input box will get a value of the type string then it will be verified if this value will be rejected as it should.
        /// </AmplitudeInputTest>
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
        /// <OffsetTest>
        /// A new application window will be created. The "ArrowUp" Button responsible for the amplitude offset will be clicked and then it will be verified if the value of amplitude will increase.
        /// </OffsetTest>
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
        /// <OffsetTest>
        /// A new application window will be created. The "ArrowDown" Button responsible for the amplitude offset will be clicked and then it will be verified if the value of amplitude will decrease.
        /// </OffsetTest>
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
