using AuthorityDomain.DomainLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AuthorityDomain.DomainLayer.Entities;

namespace AuthorityDomainTest
{
    
    
    /// <summary>
    ///This is a test class for AuthorityServiceTest and is intended
    ///to contain all AuthorityServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AuthorityServiceTest
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
        ///A test for FindAction
        ///</summary>
        [TestMethod()]
        public void FindActionTest()
        {
            var service = new AuthorityService();
            var authority = service.FindAction("Account", "ShowAllInvitationCode");
        }

        /// <summary>
        ///A test for IsAccessible
        ///</summary>
        [TestMethod()]
        public void IsAccessibleTest()
        {
            var service = new AuthorityService();
            var result = service.IsAccessible(2, 1);
        }
    }
}
