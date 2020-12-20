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
    class JsonTesting
    {
        [Test]
        public static void JsonFileIsCreate_FileExists_ReturnsTrue()
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
        [Test]
        public static void CanGetProfileIsNullError_ProfileParametersDoNotExist_ReturnsError()
        {
            //Arrange
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            //Act
            testingWindow.mSelectedTargetOnDB = null;
            testingWindow.btnInsertProfile_Click(sender, e);
            //Assert
            Assert.AreEqual(true, testingWindow.CannotInsertEmptyProfile);
        }
        [Test]
        public static void CanInsertSelectedProfile_ProfileIsAdded_ReturnsNewProfile()
        {
            //Arrange            
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            testingWindow.mSelectedTargetOnDB = "stringInputExemple";
            //Act
            testingWindow.btnInsertProfile_Click(sender, e);
            //Assert
            Assert.AreEqual(true, testingWindow.AddedToMultipleShotList);
        }
    }
    
}
