using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace WpfApp2
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    class FrequencyTesting
    {
        [Test]      
        public static void CanInputFrequency_InputIsCorrect_ReturnsTrue()
        {
            //Arrange
            var testingWindow = new MainWindow();
            //Act
            testingWindow.txtFreq.Text = Convert.ToString(50);
            //Assert
            Assert.AreEqual(true, testingWindow.mValidInput);       
        }
        [Test]
        public static void CannotInputNegativeFrequency_InputIsOutOfBoundaries_ReturnsFalse()
        {
            //Arrange
            var testingWindow = new MainWindow();
            //Act
            testingWindow.txtFreq.Text = Convert.ToString(-85);
            //Assert
            Assert.AreEqual(false, testingWindow.mValidInput);
        }
        [Test]
        public static void CannotInputOtherTypeThenLongForFrequency_InputTypeIsWrong_ReturnsFalse()
        {
            //Arrange
            var testingWindow = new MainWindow();
            //Act
            testingWindow.txtFreq.Text = Convert.ToString("b");
            //Assert
            Assert.AreEqual(false, testingWindow.mValidInput);
        }
    }
}
