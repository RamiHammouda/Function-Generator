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
    public class ResetButton
    {
        //TestName_Scenario_ExpectedBehavior
        /// <ResetTest>
        /// A new application window will be created. The "Reset" Button will be clicked and then it will be verified if the new values are equal to the default values.
        /// </ResetTest>
        [TestMethod]
        public void CanResetAllValuesToDefault_ResetIsPressed_ReturnsDefaultValues()
        {
            //Arrange
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            //Act
            testingWindow.btnReset_Click(sender, e);
            //Assert
            Assert.AreEqual(true, ((testingWindow.mFreq == 0.1) && (testingWindow.mAmpl == 5) && (testingWindow.mRate == 2) && (testingWindow.mDuration == 0) && (testingWindow.mWave == Model.WaveForm.Sine)));
        }
    }
}
