using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using WpfApp2.ViewModel;

namespace WpfApp2.SettingsTab
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    class SettingsTab
    {
        [Test]
        public static void CanEditDataBaseConnectionSettings_EnableEditClicked_CanEdit()
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
        [Test]
        public static void CanTestConnectionToDB_TestConnectionClicked_Returnstrue()
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
        //under Testing
        //[Test]
        public static void CanSaveNewProfileSettings_SaveSettingsClicked_ReturnsSettings()
        {
            //Arrange
            var testingWindow = new MainWindow();
            var targetWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow);
            object sender = null;
            RoutedEventArgs e = null;
            testingWindow.mSettingTab = SettingInfor.Instance;
            testingWindow.mSettingTab.LoadInfoToSetting();
            //Act
            testingWindow.btnSave_Click(sender, e);
            //Assert
            Assert.AreEqual(true, testingWindow.InfosSaved);
        }
    }
}
