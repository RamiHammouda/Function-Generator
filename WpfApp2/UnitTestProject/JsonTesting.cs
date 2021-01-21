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
    public class JsonTesting
    {
        //TestName_Scenario_ExpectedBehavior
        /// <JsonFileTest>
        /// A new application window will be created. The "Simulate To Json" Button will be clicked and then it will be verified if the Json File was exported.
        /// </JsonFileTest>
        [TestMethod]
        public void JsonFileIsCreate_FileExists_ReturnsTrue()
        {
            //Arrange
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            //Act
            testingWindow.btnSimulateToJson_Click(sender, e);
            //Assert
            Assert.AreEqual(true, testingWindow.exportingIsFinished);
        }

        //TestName_Scenario_ExpectedBehavior
        /// <JsonFileErrorTest>
        /// You will get an error if you try to insert an empty profile.
        /// </JsonFileErrorTest>
        [TestMethod]
        public void CanGetProfileIsNullError_ProfileParametersDoNotExist_ReturnsError()
        {
            //Arrange
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            //Act
            testingWindow.mSelectedTargetOnDB = null;
            testingWindow.btnInsertProfile_Click(sender, e);
            //Assert
            Assert.AreEqual(true, testingWindow.cannotInsertEmptyProfile);
        }

        //TestName_Scenario_ExpectedBehavior
        /// <JsonFileInsertTest>
        /// This test is responsible to verify if the user can add a new profile.
        /// </JsonFileInsertTest>
        [TestMethod]
        public void CanInsertSelectedProfile_ProfileIsAdded_ReturnsNewProfile()
        {
            //Arrange            
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            testingWindow.mSelectedTargetOnDB = "stringInputExemple";
            //Act
            testingWindow.btnInsertProfile_Click(sender, e);
            //Assert
            Assert.AreEqual(true, testingWindow.addedToMultipleShotList);
        }
    }
}
