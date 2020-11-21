using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace WpfApp2
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class Testing_SingleShot
    {
        [Test]      
        public static void CanInputFrequency_InputIsCorrect_ReturnsTrue()
        {
            //Arrange
            var neww = new MainWindow();
            //Act
            neww.mFreq = 50;
            //Assert
            Assert.AreEqual(false, neww.mValidInput);
        }
    }
}
