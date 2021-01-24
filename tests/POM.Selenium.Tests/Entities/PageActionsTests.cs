using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using POM.Selenium.Exceptions.Page;
using POM.Selenium.Exceptions.Selector;
using POM.Selenium.Tests.Config;
using POM.Selenium.Tests.Pages.TestsPages;

namespace POM.Selenium.Tests.Entities
{
    [TestClass]
    public class PageActionsTests
    {
        private delegate object ServiceProvider(Type type);
        private static ServiceProvider GetService {get; set; }
        public PageActionsTests()
        {
            TesterHostBuilder.Init();
            GetService = new ServiceProvider(TesterHostBuilder.Host.Services.GetService);
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageActions")]
        public void Should_Perfom_Click_Auto_Indexing()
        {
            var pageActions = GetService(typeof(PageActions)) as PageActions;

            pageActions
            .Requires()
            .GoToUrl("nuget.org")
            .IsOnPage()
            .Fulfill();

            pageActions
            .PerformClick()
            .PerformClick()
            .PerformClick()
            .PerformClick();

            Assert.AreEqual(4, pageActions.ClickIndex);
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageActions")]
        public void Should_Perfom_Click_Button_Index()
        {
            var pageActions = GetService(typeof(PageActions)) as PageActions;

            pageActions
            .Requires()
            .GoToUrl("nuget.org")
            .IsOnPage()
            .Fulfill();

            pageActions
            .PerformClick(buttonIndex: 0)
            .PerformClick(buttonIndex: 1)
            .PerformClick(buttonIndex: 2)
            .PerformClick(buttonIndex: 3)
            .PerformClick(buttonIndex: 4);

            var amazonLambdaTools = pageActions.Execute((d)=>d.FindElement(By.CssSelector("[data-package-id='Amazon.Lambda.Tools']")).Text);

            Assert.AreEqual(amazonLambdaTools, "Amazon.Lambda.Tools");
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageActions")]
        public void Should_Perfom_Instance_Click()
        {
            var pageActions = GetService(typeof(PageActions)) as PageActions;

            pageActions
            .Requires()
            .GoToUrl("nuget.org/packages")
            .IsOnPage()
            .Fulfill();

            var searchValues = new[]{ "Microsoft.Extensions.Logging" };

            pageActions
            .GetTableInstances(searchValues)
            .PerformInstanceClick(selector: "a");
            Thread.Sleep(2000);

            var elements = pageActions.Execute((d) => d.FindElements(By.CssSelector("[aria-label='Package details info']")));

            Assert.IsTrue(elements.Any());
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageActions")]
        public void Should_Perfom_Instance_Click_And_Throws_Page_Exception()
        {
            var pageActions = GetService(typeof(PageActions)) as PageActions;

            pageActions
            .Requires()
            .GoToUrl("nuget.org/packages")
            .IsOnPage()
            .Fulfill();

            Assert.ThrowsException<PageException>(() => pageActions.PerformInstanceClick(selector: "a"));
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageActions")]
        public void Should_Perfom_Click_Auto_Indexing_And_Throws_Selector_Exception()
        {
            var pageActions = GetService(typeof(PageActions)) as PageActions;

            pageActions
            .Requires()
            .GoToUrl("nuget.org")
            .IsOnPage()
            .Fulfill();            

            Assert.ThrowsException<SelectorException>(() =>
            {
                pageActions
                .PerformClick()
                .PerformClick()
                .PerformClick()
                .PerformClick()
                .PerformClick()
                .PerformClick();
            });
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageActions")]
        public void Should_Perfom_Click_Button_Index_And_Throws_Selector_Exception()
        {
            var pageActions = GetService(typeof(PageActions)) as PageActions;

            pageActions
            .Requires()
            .GoToUrl("nuget.org")
            .IsOnPage()
            .Fulfill();

            Assert.ThrowsException<SelectorException>(() => pageActions.PerformClick(buttonIndex: 10000));
        }
    }
}