using IdeaDomain.InfrastructureLayer.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using IdeaDomain.DomainLayer.Entities;
using System.Data;

namespace IdeaDomainTest
{


    /// <summary>
    ///This is a test class for AnalyzerRepositoryTest and is intended
    ///to contain all AnalyzerRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AnalyzerRepositoryTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for GetMyAnalyzerData
        ///</summary>
        [TestMethod()]
        public void GetMyAnalyzerDataTest()
        {
            var target = new AnalyzerRepository(); // TODO: Initialize to an appropriate value
            //Analyzer analyzer = new Analyzer
            //{
            //    AnalyzerName = "hehe",
            //    SelectQuery = "Select Customer.Name ",
            //    JoinQuery = " [3][ left join ][2][ on ConsumptionRecord.CardId = GymCard.RowId ][ left join ][1] [ on GymCard.CustomerId = Customer.RowId ]",
            //    WhereQuery = "Where Customer.RowId = 1"
            //}; // TODO: Initialize to an appropriate value
            var analyzer = new Analyzer
            {
                AnalyzerName = "Times customer go to the Gym this month",
                SelectQuery =
                    "Select Customer.CustomerId , " +
                    "sum ( case when ( DATEPART ( mm , ConsumptionRecord.StartTime ) = DATEPART ( mm , GETDATE() ) "+
                    "and year ( ConsumptionRecord.StartTime ) = year ( GETDATE() ) ) then 1 else 0 end ) as times ",
                JoinQuery = " [ 3 ] [ right join ] [ 2 ] [ on ConsumptionRecord.CardId = GymCard.RowId ] [ right join ] [1] [ on GymCard.CustomerId = Customer.RowId ]",
                WhereQuery = "Where 1 = 1 Group by Customer.CustomerId"
            }; // TODO: Initialize to an appropriate value
            analyzer = new Analyzer
            {
                AnalyzerName = "Times customer go to the Gym this year",
                SelectQuery =
                    "Select Customer.CustomerId, " +
                    "sum(case when (year(ConsumptionRecord.StartTime)=year(GETDATE())) then 1 else 0 end) as times ",
                JoinQuery = " [3][ right join ][2][ on ConsumptionRecord.CardId = GymCard.RowId ][ right join ][1] [ on GymCard.CustomerId = Customer.RowId ]",
                WhereQuery = "Where 1=1 Group by Customer.CustomerId"
            }; // TODO: Initialize to an appropriate value
            DataTable expected = null; // TODO: Initialize to an appropriate value
            AnalyzerDetail actual;
            actual = target.GetAnalyzerData(analyzer);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
