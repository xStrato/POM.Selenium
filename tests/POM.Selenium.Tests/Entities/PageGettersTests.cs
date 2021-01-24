using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POM.Selenium.Exceptions.Page;
using POM.Selenium.Exceptions.Selector;
using POM.Selenium.Tests.Config;
using POM.Selenium.Tests.Pages.TestsPages;


namespace POM.Selenium.Tests.Entities
{
    [TestClass]
    public class PageGettersTests
    {
        private delegate object ServiceProvider(Type type);
        private static ServiceProvider GetService {get; set; }
        public PageGettersTests()
        {
            TesterHostBuilder.Init();
            GetService = new ServiceProvider(TesterHostBuilder.Host.Services.GetService);
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageGetters")]
        public void Should_Return_A_Generic_Model()
        {
            var pageGetters = GetService(typeof(PageActions)) as PageActions;

            pageGetters
            .Requires()
            .GoToUrl("nuget.org")
            .IsOnPage()
            .Fulfill();

            var model = pageGetters
            .GetPageValues<TestModel>()
            .PageValues as TestModel;

            Assert.IsInstanceOfType(model, typeof(TestModel));
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageGetters")]
        public void Should_Get_Page_Values()
        {
            var pageGetters = GetService(typeof(PageActions)) as PageActions;

            pageGetters
            .Requires()
            .GoToUrl("nuget.org")
            .IsOnPage()
            .Fulfill();

            var model = pageGetters.GetPageValues().PageValues;

            Assert.AreEqual("About", model[0]);
            Assert.AreEqual("Terms of Use", model[1]);
            Assert.AreEqual("Privacy Policy", model[2]);
            Assert.AreEqual("Manage Cookies", model[3]);
            Assert.AreEqual("Trademarks", model[4]);
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageGetters")]
        public void Should_Get_Page_Values_With_Generic_Model()
        {
            var pageGetters = GetService(typeof(PageActions)) as PageActions;

            pageGetters
            .Requires()
            .GoToUrl("nuget.org")
            .IsOnPage()
            .Fulfill();

            var model = pageGetters
            .GetPageValues<TestModel>()
            .PageValues as TestModel;

            Assert.AreEqual("About", model.About);
            Assert.AreEqual("Terms of Use", model.TermsOfUse);
            Assert.AreEqual("Privacy Policy", model.PrivacyPolicy);
            Assert.AreEqual("Manage Cookies", model.ManageCookies);
            Assert.AreEqual("Trademarks", model.Trademarks);
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageGetters")]
        public void Should_Get_Instances()
        {
            var pageGetters = GetService(typeof(PageActions)) as PageActions;

            pageGetters
            .Requires()
            .GoToUrl("nuget.org/packages")
            .IsOnPage()
            .Fulfill();

            pageGetters.GetInstances("Microsoft.Extensions.Logging","AutoMapper","AWSSDK");

            Assert.AreEqual(3, pageGetters.Instances.Count);
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageGetters")]
        public void Should_Get_Instance()
        {
            var pageGetters = GetService(typeof(PageActions)) as PageActions;

            pageGetters
            .Requires()
            .GoToUrl("nuget.org/packages")
            .IsOnPage()
            .Fulfill();

            pageGetters.GetInstance("Serilog");

            Assert.AreEqual(1, pageGetters.Instances.Count);
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageGetters")]
        public void Should_Get_Table_Instances()
        {
            var pageGetters = GetService(typeof(PageActions)) as PageActions;

            pageGetters
            .Requires()
            .GoToUrl("nuget.org/packages")
            .IsOnPage()
            .Fulfill();

            var searchValues = new[]
            { 
                "Newtonsoft.Json",
                "by: dotnetfoundation jamesnk newtonsoft",
                "Json.NET is a popular high-performance JSON framework for .NET"
            };

            pageGetters.GetTableInstances(searchValues);

            Assert.AreEqual(1, pageGetters.TableInstances.Count);
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageGetters")]
        public void Should_Get_Table_Values()
        {
            var pageGetters = GetService(typeof(PageActions)) as PageActions;

            pageGetters
            .Requires()
            .GoToUrl("nuget.org/packages")
            .IsOnPage()
            .Fulfill();

            var searchValues = new[]
            { 
                "Newtonsoft.Json",
                "by: dotnetfoundation jamesnk newtonsoft",
                "Json.NET is a popular high-performance JSON framework for .NET"
            };

            pageGetters
            .GetTableInstances(searchValues)
            .GetTableValues(innerTableSelector:"div");

            Assert.AreEqual(5, pageGetters.TableValues[0].Length);
        }


        [TestMethod]
        [TestCategory("Entities/Pages/PageGetters")]
        public void Should_Get_Page_Values_With_Generic_Model_And_Thows_Selector_Exception()
        {
            var pageGetters = GetService(typeof(PageActions)) as PageActions;

            pageGetters
            .Requires()
            .GoToUrl("nuget.org")
            .IsOnPage()
            .Fulfill();
 
            Assert.ThrowsException<SelectorException>(() => pageGetters.GetPageValues<FailTestModel>());
        }


        [TestMethod]
        [TestCategory("Entities/Pages/PageGetters")]
        public void Should_Get_Table_Instances_And_Thows_Selector_Exception()
        {
            var pageGetters = GetService(typeof(PageActions)) as PageActions;

            pageGetters
            .Requires()
            .GoToUrl("nuget.org/packages")
            .IsOnPage()
            .Fulfill();

            var searchValues = new[]
            { 
                "Newtonsoft.Json",
                "by: dotnetfoundation jamesnk newtonsoft",
                "Json.NET is a popular high-performance JSON framework for .NET",
                "Json.NET is a popular high-performance JSON framework for .NET",
                "Json.NET is a popular high-performance JSON framework for .NET",
                "Json.NET is a popular high-performance JSON framework for .NET"
            };
            Assert.ThrowsException<SelectorException>(() =>  pageGetters.GetTableInstances(searchValues));
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageGetters")]
        public void Should_Get_Table_Instances_And_Thows_Argument_Exception()
        {
            var pageGetters = GetService(typeof(PageActions)) as PageActions;

            pageGetters
            .Requires()
            .GoToUrl("nuget.org/packages")
            .IsOnPage()
            .Fulfill();

            Assert.ThrowsException<ArgumentException>(() =>  pageGetters.GetTableInstances(new[]{"", ""}));
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageGetters")]
        public void Should_Get_Table_Values_And_Throws_Page_Exception()
        {
            var pageGetters = GetService(typeof(PageActions)) as PageActions;

            pageGetters
            .Requires()
            .GoToUrl("nuget.org/packages")
            .IsOnPage()
            .Fulfill();

            Assert.ThrowsException<PageException>(() => pageGetters.GetTableValues(innerTableSelector:"div"));
        }
    }
}