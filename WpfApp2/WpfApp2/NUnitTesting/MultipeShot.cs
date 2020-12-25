using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace WpfApp2.MultipleShot
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    class MultipeShot
    {
        //TestName_Scenario_ExpectedBehavior
        /// <AddItemTest>
        /// A new application window will be created. The "Add Item" Button will be clicked and then it will be verified if the item was added.
        /// </AddItemTest>
        [Test]
        public static void CanAddNewItem_AddItemClicked_CreatesItem()
        {
            //Arrange
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            //Act
            testingWindow.MenuItemAdd_Click(sender,e);
            //Assert
            Assert.AreEqual(true, testingWindow.ItemAdded);
        }

        /// <DeleteItemTest>
        /// A new application window will be created. The "Delete Item" Button will be clicked and then it will be verified if the item was deleted.
        /// </DeleteItemTest>
        //Under testing
        //[Test]
        public static void CanDeleteAnItem_DeleteItemClicked_DeletesItem()
        {
            //Arrange
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            //Act
            testingWindow.MenuItemDel_Click(sender, e);
            //Assert
            Assert.AreEqual(true, testingWindow.ItemDeleted);
        }

        //Under Testing
        //[Test]
        public static void NoItemsAdded_SaveToFileClicked_ErrorMessage()
        {
            //Arrange
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            //Act
            testingWindow.btnMSimulateToJson_Click(sender, e);
            //Assert
            Assert.AreEqual(true, testingWindow.NotEnouhItems);
        }

        /// <SaveAllItems>
        /// A new application window will be created. The "Save To File" Button will be clicked and then it will be verified if the items can be saved.
        /// </SaveAllItems>
        [Test]
        public static void CanSaveAllItemsTojson_SaveToFileClicked_ItemsSaved()
        {
            //Arrange
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            //Act
            testingWindow.btnMSimulateToJson_Click(sender, e);
            //Assert
            Assert.AreEqual(true, testingWindow.ItemsAreSaved);
        }

        // Under Testing
        //[Test]
        //public static void CanGetConnectionToDBFailedError_SaveToDBClicked_ErrorMessage()
        //{
        //    //Arrange
        //    var testingWindow = new MainWindow();
        //    object sender = null;
        //    RoutedEventArgs e = null;
        //    //Act
        //    testingWindow.mSettingTab.CheckConnection();
        //    testingWindow.btnMSimuToDB_Click(sender, e);
        //    //Assert
        //    Assert.AreEqual(true, testingWindow.ConnectionToDBError);
        //}

        // Under Testing
        //[Test]
        //public static void CanSaveAllItemsToDB_SaveToDBClicked_ItemsSaved()
        //{
        //    //Arrange
        //    var testingWindow = new MainWindow();
        //    object sender = null;
        //    RoutedEventArgs e = null;
        //    //Act
        //    testingWindow.btnMSimuToDB_Click(sender, e);
        //    //Assert
        //    Assert.AreEqual(true, testingWindow.ItemsAreSavedToDB);
        //}

        /// <ViewingDataBase>
        /// A new application window will be created. The "View All Database" Button will be clicked and then it will be verified if the user can view the database.
        /// </ViewingDataBase>
        [Test]
        public static void CanViewDataBase_ViewAllDataBaseClicked_NewWindowopened()
        {
            //Arrange
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            //Act
            testingWindow.btnViewDatabase_Click(sender, e);
            //Assert
            Assert.AreEqual(true, testingWindow.DataBaseWindowOpened);
        }
    }
}
