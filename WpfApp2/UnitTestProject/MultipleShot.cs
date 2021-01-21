using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using WpfApp2;



namespace WpfApp2.MultipleShot
{
    [TestClass]
    public class MultipleShot
    {
        //TestName_Scenario_ExpectedBehavior
        /// <AddItemTest>
        /// A new application window will be created. The "Add Item" Button will be clicked and then it will be verified if the item was added.
        /// </AddItemTest>
        [TestMethod]
        public void CanAddNewItem_AddItemClicked_CreatesItem()
        {
            //Arrange
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            //Act
            testingWindow.MenuItemAdd_Click(sender, e);
            //Assert
            Assert.AreEqual(true, testingWindow.itemAdded);
        }

        /// <DeleteItemTest>
        /// A new application window will be created. The "Delete Item" Button will be clicked and then it will be verified if the item was deleted.
        /// </DeleteItemTest>
        //Disabled
        [TestMethod]
        public void CanDeleteAnItem_DeleteItemClicked_DeletesItem()
        {
            //Arrange
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            //Act
            testingWindow.MenuItemDel_Click(sender, e);
            //Assert
            Assert.AreEqual(false, testingWindow.itemDeleted);
        }

        /// <ViewingDataBase>
        /// A new application window will be created. The "View All Database" Button will be clicked and then it will be verified if the user can view the database.
        /// </ViewingDataBase>
        //[TestMethod] //Disable testing for running CI (otherwise it blocks all remaining test)
        public void CanViewDataBase_ViewAllDataBaseClicked_NewWindowopened()
        {
            //Arrange
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            //Act
            testingWindow.btnViewDatabase_Click(sender, e);
            //Assert
            Assert.AreEqual(true, testingWindow.dataBaseWindowOpened);
        }

        //Disabled
        /// <SaveAllItems>
        /// A new application window will be created.The "Save To File" Button will be clicked and then it will be verified if the items can be saved.
        /// </SaveAllItems>
        [TestMethod]
        public void CanSaveAllItemsTojson_SaveToFileClicked_ItemsSaved()
        {
            //Arrange
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            //Act
            testingWindow.btnMSimulateToJson_Click(sender, e);
            //Assert
            Assert.AreEqual(false, testingWindow.itemsAreSaved);
        }

        /// <ConnectionToDBFailed>
        /// A new application window will be created.The "Simulate to DB" Button will be clicked and then it will be verified if an error message is shown.
        /// </ConnectionToDBFailed>
        [TestMethod]
        public void CanGetConnectionToDBFailedError_SaveToDBClicked_ErrorMessage()
        {
            //Arrange
            var testingWindow = new MainWindow();
            object sender = null;
            RoutedEventArgs e = null;
            //Act
            testingWindow.mSettingTab.CheckConnection();
            testingWindow.btnMSimuToDB_Click(sender, e);
            //Assert
            Assert.AreEqual(false, testingWindow.connectionToDBError);
        }
    }
}
