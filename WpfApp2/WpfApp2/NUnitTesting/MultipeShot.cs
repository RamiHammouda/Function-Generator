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
        [Test]
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
    }
}
