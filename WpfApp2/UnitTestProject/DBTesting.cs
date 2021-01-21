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
using WpfApp2.Model;
using WpfApp2.ViewModel;

namespace WpfApp2.DBTesting
{
    [TestClass]
    public class DB_Test
    {
        //TestName_Scenario_ExpectedBehavior
        /// <WriteToDBTest>
        /// This Test will verify if it is possible to start writing in the DB.
        /// </WriteToDBTest>
        [TestMethod]
        public void CanStartWriteToDB_ConnectionIsTrue_FinishedWriting() //brauche ein connectionstring and query stgring bs
        {
            //Arrange
            GenerateSignalData SignalClassExemple = new GenerateSignalData();
            //Act
            SignalClassExemple.StartWriteToDB();
            //Assert
            //Assert.AreEqual(true, SignalClassExemple.writingIsFinished);
        }

        // Under Testing
        [TestMethod]
        public void CanChooseDBTarget_ChooseBetweenTargets_TargetChosen() //brauche ein connectionstring and query stgring bs
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

        //// Under Testing
        [TestMethod]
        public void CanGetNoTargetChoseError_ChooseBetweenTargets_NoTargetChosen() //brauche ein connectionstring and query stgring bs
        {
            //Arrange            
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            testingWindow.mSelectedTargetOnDB = null;
            //Act
            testingWindow.btnInsertProfile_Click(sender, e);
            //Assert
            Assert.AreEqual(true, testingWindow.cannotInsertEmptyProfile);
        }
    }
}
