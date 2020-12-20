using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using WpfApp2;
using WpfApp2.ViewModel;

namespace WpfApp2.DBTesting
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    class DB_Test
    {
        //[Test]
        public static void CanStartWriteToDB_ConnectionIsTrue_FinishedWriting() //brauche ein connectionstring and query stgring bs
        {
            //Arrange
            GenerateSignalData SignalClassExemple = new GenerateSignalData();
            //Act
            SignalClassExemple.StartWriteToDB();
            //Assert
            Assert.AreEqual(true, SignalClassExemple.WritingIsFinished);
        }
        //[Test]
        //public static void CanChooseDBTarget_ChooseBetweenTargets_TargetChosen() //brauche ein connectionstring and query stgring bs
        //{
        //    //Arrange            
        //    var testingWindow = new MainWindow();
        //    object sender = null;
        //    RoutedEventArgs e = null;
        //    testingWindow.mSelectedTargetOnDB = "stringInputExemple";
        //    //Act
        //    testingWindow.btnInsertProfile_Click(sender, e);
        //    //Assert
        //    Assert.AreEqual(true, testingWindow.AddedToMultipleShotList);
        //}
        //[Test]
        //public static void CanGetNoTargetChoseError_ChooseBetweenTargets_NoTargetChosen() //brauche ein connectionstring and query stgring bs
        //{
        //    //Arrange            
        //    var testingWindow = new MainWindow();
        //    object sender = null;
        //    RoutedEventArgs e = null;
        //    testingWindow.mSelectedTargetOnDB = "stringInputExemple";
        //    //Act
        //    testingWindow.btnInsertProfile_Click(sender, e);
        //    //Assert
        //    Assert.AreEqual(true, testingWindow.AddedToMultipleShotList);
        //}    
    }
}
