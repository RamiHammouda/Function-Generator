﻿using NUnit.Framework;
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
    }
}
