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

namespace WpfApp2.SettingsTab
{
    [TestClass]
    public class SeetingsTab
    {
        //TestName_Scenario_ExpectedBehavior
        /// <EnableChangesToDataBaseSettings>
        /// A new application window will be created. The "Enable Edit" Button will be clicked and then it will be verified if the user can input new data.
        /// </EnableChangesToDataBaseSettings>
        [TestMethod]
        public void CanEditDataBaseConnectionSettings_EnableEditClicked_CanEdit()
        {
            //Arrange
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            //Act
            testingWindow.btnEnableEdit_Click(sender, e);
            //Assert
            Assert.AreEqual(true, testingWindow.EditIsEnabled);
        }

        //TestName_Scenario_ExpectedBehavior
        /// <ConnectionToDBTest>
        /// A new application window will be created. The "Test Conenction" Button will be clicked and then it will be verified if a connection to the DB is possible.
        /// </ConnectionToDBTest>
        [TestMethod]
        public void CanTestConnectionToDB_TestConnectionClicked_Returnstrue()
        {
            //Arrange
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            //Act
            testingWindow.btnTestConn_Click(sender, e);
            //Assert
            Assert.AreEqual(true, testingWindow.ConnectionTested);

        }
    }
}
