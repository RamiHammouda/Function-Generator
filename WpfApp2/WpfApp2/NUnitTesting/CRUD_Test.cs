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
using WpfApp2.Model;

namespace WpfApp2.CRUDTesting
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    class CRUD_Test
    {
        //TestName_Scenario_ExpectedBehavior
        /// <ConnectionTest>
        /// A new class will be created. This test will verify if the connection is possible.
        /// </ConnectionTest>
        //[Test]
        public static void CanConnect_Connected_ConnectionBuilt() //brauche ein connectionstring and query stgring bs
        {
            //Arrange
            MyDBEntity DBClassExemple = new MyDBEntity();
            //Act
            string connectionString = "a";
            string queryString = "b";
            DBClassExemple.Connect(connectionString, queryString);
            //Assert
            Assert.AreEqual(true, DBClassExemple.CanConnectToDB);
        }

        //TestName_Scenario_ExpectedBehavior
        /// <ReadingTest>
        /// A new class will be created. This test will verify if the user can read from the DB.
        /// </ReadingTest>
        //[Test]
        public static void CanRead_ReaderIsTrue_ReadingInTheDB() //brauche ein connectionstring and query stgring bs
        {
            //Arrange
            MyDBEntity DBClassExemple = new MyDBEntity();
            //Act
            string connectionString = "a";
            string queryString = "b";
            DBClassExemple.Reader(connectionString, queryString);
            //Assert
            Assert.AreEqual(true, DBClassExemple.CanReadInDB);
        }

        //TestName_Scenario_ExpectedBehavior
        /// <GetColumnsTest>
        /// A new class will be created. This test will verify if get the shown columns.
        /// </GetColumnsTest>
        //[Test]
        public static void CanGetColumnsFromDB_ConnectionIsTrue_ColumnsObtained() //brauche ein connectionstring and query stgring bs
        {
            //Arrange
            MyDBEntity DBClassExemple = new MyDBEntity();
            //Act
            DBClassExemple.GetColumns();           
            //Assert
            Assert.AreEqual(true, DBClassExemple.CanReadInDB);
        }

        //TestName_Scenario_ExpectedBehavior
        /// <IntInsertionTest>
        /// A new class will be created. This test will verify if get a new Input of the type int is possible.
        /// </IntInsertionTest>
        //[Test]
        public static void CanInsertIntInDB_InsertIsPossibleInput_InsertingInTheDB() //brauche ein connectionstring and query stgring bs
        {
            //Arrange
            MyDBEntity DBClassExemple = new MyDBEntity();
            //Act
            int x = 6;
            DBClassExemple.Insert(x);           
            //Assert
            Assert.AreEqual(true, DBClassExemple.NewIntIsInserted);
        }

        //TestName_Scenario_ExpectedBehavior
        /// <DoubleInsertionTest>
        /// A new class will be created. This test will verify if get a new Input of the type double is possible.
        /// </DoubleInsertionTest>
        //[Test]
        public static void CanInsertDoubleInDB_DoubleIsPossibleInput_InsertingInTheDB() //brauche ein connectionstring and query stgring bs
        {
            //Arrange
            MyDBEntity DBClassExemple = new MyDBEntity();
            //Act
            double x = 6.0;
            DBClassExemple.Insert(x);
            //Assert
            Assert.AreEqual(true, DBClassExemple.NewDoubleIsInserted);
        }

        //TestName_Scenario_ExpectedBehavior
        /// <LongInsertionTest>
        /// A new class will be created. This test will verify if get a new Input of the type long is possible.
        /// </LongInsertionTest>
        //[Test]
        public static void CanInsertLongInDB_LongIsPossibleInput_InsertingInTheDB() //brauche ein connectionstring and query stgring bs
        {
            //Arrange
            MyDBEntity DBClassExemple = new MyDBEntity();
            //Act
            long x = 2^500 ;
            DBClassExemple.Insert(x);
            //Assert
            Assert.AreEqual(true, DBClassExemple.NewLongIsInserted);
        }

        //TestName_Scenario_ExpectedBehavior
        /// <uShortInsertionTest>
        /// A new class will be created. This test will verify if get a new Input of the type uShort is possible.
        /// </uShortInsertionTest>
        //[Test]
        public static void CanuShortLongInDB_uShortIsPossibleInput_InsertingInTheDB() //brauche ein connectionstring and query stgring bs
        {
            //Arrange
            MyDBEntity DBClassExemple = new MyDBEntity();
            //Act
            ushort x = 32765;
            DBClassExemple.Insert(x);
            //Assert
            Assert.AreEqual(true, DBClassExemple.NewuShortIsInserted);
        }

        //TestName_Scenario_ExpectedBehavior
        /// <sByteInsertionTest>
        /// A new class will be created. This test will verify if get a new Input of the type sByte is possible.
        /// </sByteInsertionTest>
        //[Test]
        public static void CanInsertsByteInDB_sByteIsPossibleInput_InsertingInTheDB() //brauche ein connectionstring and query stgring bs
        {
            //Arrange
            MyDBEntity DBClassExemple = new MyDBEntity();
            //Act
            sbyte x = 127;
            DBClassExemple.Insert(x);
            //Assert
            Assert.AreEqual(true, DBClassExemple.NewsByteIsInserted);
        }
    }
}
