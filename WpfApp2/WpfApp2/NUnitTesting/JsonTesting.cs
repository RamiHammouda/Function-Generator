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
        //TestName_Scenario_ExpectedBehavior
        /// <JsonFileTest>
        /// A new application window will be created. The "Simulate To Json" Button will be clicked and then it will be verified if the Json File was exported.
        /// </JsonFileTest>
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
    }
}
