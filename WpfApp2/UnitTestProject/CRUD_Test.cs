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

namespace WpfApp2.CRUDTesting
{
    [TestClass]
    public class CRUD_Test
    {
        //TestName_Scenario_ExpectedBehavior
        /// <ConnectionTest>
        /// A new class will be created. This test will verify if the connection is possible.
        /// </ConnectionTest>
        [TestMethod]
        public void CanConnect_Connected_ConnectionBuilt() //brauche ein connectionstring and query stgring bs
        {
            //Arrange
            MyDBEntity DBClassExemple = new MyDBEntity();
            //Act
            string server = "fgdb-f2-htw.selfhost.co";
            string dataBase = "plc_data";
            string username = "test username";
            string password = "test password";
            string connectionString = "SERVER=" + server + "; PORT =" + 3306 + "; DATABASE=" + dataBase + ";" + "UID=" + username + ";" + "PASSWORD=" + password + ";";
            string queryString = $"SELECT `COLUMN_NAME` FROM `INFORMATION_SCHEMA`.`COLUMNS`  WHERE `TABLE_SCHEMA`= '{dataBase}' AND `TABLE_NAME`= '{DBClassExemple.tableName}' ORDER BY table_name, ordinal_position;";
            DBClassExemple.Connect(connectionString, queryString);
            //Assert
            Assert.AreEqual(true, DBClassExemple.canConnectToDB);
        }

        //TestName_Scenario_ExpectedBehavior
        /// <ReadingTest>
        /// A new class will be created. This test will verify if the user can read from the DB.
        /// </ReadingTest>
        //[TestMethod]
        public void CanRead_ReaderIsTrue_ReadingInTheDB() //brauche ein connectionstring and query stgring bs
        {
            //Arrange
            MyDBEntity DBClassExemple = new MyDBEntity();
            //Act
            string connectionString = "SERVER=" + DBClassExemple.serverIP + "; PORT =" + DBClassExemple.portNumber + "; DATABASE=" + DBClassExemple.database + ";" + "UID=" + DBClassExemple.username + ";" + "PASSWORD=" + DBClassExemple.password + ";";
            string queryString = $"SELECT `COLUMN_NAME` FROM `INFORMATION_SCHEMA`.`COLUMNS`  WHERE `TABLE_SCHEMA`= '{DBClassExemple.database}' AND `TABLE_NAME`= '{DBClassExemple.tableName}' ORDER BY table_name, ordinal_position;";
            DBClassExemple.Reader(connectionString, queryString);
            //Assert
            Assert.AreEqual(true, DBClassExemple.canReadInDB);
        }

        //TestName_Scenario_ExpectedBehavior
        /// <GetColumnsTest>
        /// A new class will be created. This test will verify if get the shown columns.
        /// </GetColumnsTest>
        //[TestMethod]
        public void CanGetColumnsFromDB_ConnectionIsTrue_ColumnsObtained() //brauche ein connectionstring and query stgring bs
        {
            //Arrange
            MyDBEntity DBClassExemple = new MyDBEntity();
            //Act
            DBClassExemple.GetColumns();
            //Assert
            Assert.AreEqual(true, DBClassExemple.canGetColumnsFromDB);
        }

        //TestName_Scenario_ExpectedBehavior
        /// <IntInsertionTest>
        /// A new class will be created. This test will verify if get a new Input of the type int is possible.
        /// </IntInsertionTest>
        [TestMethod]
        public void CanInsertIntInDB_InsertIsPossibleInput_InsertingInTheDB() //brauche ein connectionstring and query stgring bs
        {
            //Arrange
            MyDBEntity DBClassExemple = new MyDBEntity();
            //Act
            int x = 6;
            DBClassExemple.Insert(x);
            //Assert
            Assert.AreEqual(true, DBClassExemple.newIntIsInserted);
        }

        //TestName_Scenario_ExpectedBehavior
        /// <DoubleInsertionTest>
        /// A new class will be created. This test will verify if get a new Input of the type double is possible.
        /// </DoubleInsertionTest>
        [TestMethod]
        public void CanInsertDoubleInDB_DoubleIsPossibleInput_InsertingInTheDB() //brauche ein connectionstring and query stgring bs
        {
            //Arrange
            MyDBEntity DBClassExemple = new MyDBEntity();
            //Act
            double x = 6.0;
            DBClassExemple.Insert(x);
            //Assert
            Assert.AreEqual(true, DBClassExemple.newDoubleIsInserted);
        }

        //TestName_Scenario_ExpectedBehavior
        /// <LongInsertionTest>
        /// A new class will be created. This test will verify if get a new Input of the type long is possible.
        /// </LongInsertionTest>
        [TestMethod]
        public void CanInsertLongInDB_LongIsPossibleInput_InsertingInTheDB() //brauche ein connectionstring and query stgring bs
        {
            //Arrange
            MyDBEntity DBClassExemple = new MyDBEntity();
            //Act
            long x = 2 ^ 500;
            DBClassExemple.Insert(x);
            //Assert
            Assert.AreEqual(true, DBClassExemple.newLongIsInserted);
        }

        //TestName_Scenario_ExpectedBehavior
        /// <uShortInsertionTest>
        /// A new class will be created. This test will verify if get a new Input of the type uShort is possible.
        /// </uShortInsertionTest>
        [TestMethod]
        public void CanuShortLongInDB_uShortIsPossibleInput_InsertingInTheDB() //brauche ein connectionstring and query stgring bs
        {
            //Arrange
            MyDBEntity DBClassExemple = new MyDBEntity();
            //Act
            ushort x = 32765;
            DBClassExemple.Insert(x);
            //Assert
            Assert.AreEqual(true, DBClassExemple.newUShortIsInserted);
        }

        //TestName_Scenario_ExpectedBehavior
        /// <sByteInsertionTest>
        /// A new class will be created. This test will verify if get a new Input of the type sByte is possible.
        /// </sByteInsertionTest>
        [TestMethod]
        public void CanInsertsByteInDB_sByteIsPossibleInput_InsertingInTheDB() //brauche ein connectionstring and query stgring bs
        {
            //Arrange
            MyDBEntity DBClassExemple = new MyDBEntity();
            //Act
            sbyte x = 127;
            DBClassExemple.Insert(x);
            //Assert
            Assert.AreEqual(true, DBClassExemple.newsByteIsInserted);
        }
    }
}
