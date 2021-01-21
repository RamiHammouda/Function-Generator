using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Windows;
using System.IO;
using WpfApp2;

namespace WpfApp2.SingleShot
{
    [TestClass]
    public class FrequencyTesting
    {
        //TestName_Scenario_ExpectedBehavior
        /// <FrequencyInputTest>
        /// A new application window will be created. The frequency input box will get the value of "50" then it will be verified if this value will be accepted as a valid input.
        /// </FrequencyInputTest>
        [TestMethod]
        public void CanInputFrequency_InputIsCorrect_ReturnsTrue()
        {
            //Arrange
            var testingWindow = new MainWindow();
            //Act
            testingWindow.mFreq = 50;
            //Assert
            Assert.AreEqual(false, testingWindow.mValidInput);
        }

        /// <FrequencyInputTest>
        /// A new application window will be created. The frequency input box will get the value of "-85" then it will be verified if this value will be rejected.
        /// </FrequencyInputTest>
        [TestMethod]
        public void CannotInputNegativeFrequency_InputIsOutOfBoundaries_ReturnsFalse()
        {
            //Arrange
            var testingWindow = new MainWindow();
            //Act
            testingWindow.mFreq = -85;
            //Assert
            Assert.AreEqual(false, testingWindow.mValidInput);
        }

        /// <FrequencyInputTest>
        /// A new application window will be created. The frequency input box will get a value of the type string then it will be verified if this value will be rejected.
        /// </FrequencyInputTest>
        //[TestMethod]
        public void CannotInputOtherTypeThenLongForFrequency_InputTypeIsWrong_ReturnsFalse()
        {
            //Arrange
            var testingWindow = new MainWindow();
            //Act
            testingWindow.mFreq = Convert.ToInt32("b");
            //Assert
            Assert.AreEqual(false, testingWindow.mValidInput);
        }

        /// <OffsetTest>
        /// A new application window will be created. The "ArrowUp" Button responsible for the frequency offset will be clicked and then it will be verified if the value of frequency will increase.
        /// </OffsetTest>
        [TestMethod]
        public void CanIncreaseFrequencyWithOffset_PlusButtonPressed_ReturnsBiggerValue()
        {
            //Arrange
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            //Act
            testingWindow.btnFreqPlus_Click(sender, e);
            //Assert
            Assert.AreEqual(true, testingWindow.offsetFreqValueIncreased);
        }

        /// <OffsetTest>
        /// A new application window will be created. The "ArrowDown" Button responsible for the frequency offset will be clicked and then it will be verified if the value of frequency will decrease.
        /// </OffsetTest>
        [TestMethod]
        public void CanDecreaseFrequencyWithOffset_MinusButtonPressed_ReturnsLowerValue()
        {
            //Arrange
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            //Act
            testingWindow.btnFreqMinus_Click(sender, e);
            //Assert
            Assert.AreEqual(true, testingWindow.offsetFreqValueDecreased);
        }
    }
}
