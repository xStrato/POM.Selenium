using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POM.Selenium.Exceptions.Selector;
using POM.Selenium.Tests.Config;
using POM.Selenium.Tests.Pages.TestsPages;

namespace POM.Selenium.Tests.Entities
{
    [TestClass]
    public class PageEvaluatorTests
    {
        private delegate object ServiceProvider(Type type);
        private static ServiceProvider GetService {get; set; }
        public PageEvaluatorTests()
        {
            TesterHostBuilder.Init();
            GetService = new ServiceProvider(TesterHostBuilder.Host.Services.GetService);
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageEvaluator")]
        public void Should_Evaluate_Has_Table_Results()
        {
            var pageEvaluator = GetService(typeof(PageActions)) as PageActions;

            pageEvaluator
            .Requires()
            .GoToUrl("nuget.org/packages")
            .IsOnPage()
            .Fulfill();

            Assert.IsTrue(pageEvaluator.HasTableResults());
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageEvaluator")]
        public void Should_Evaluate_Has_Erros_On_Page()
        {
            var pageEvaluator = GetService(typeof(PageActions)) as PageActions;

            pageEvaluator
            .Requires()
            .GoToUrl("nuget.org/packages")
            .IsOnPage()
            .Fulfill();

            Assert.IsTrue(pageEvaluator.HasErrosOnPage());
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageEvaluator")]
        public void Should_Evaluate_Error_Messages()
        {
            var pageEvaluator = GetService(typeof(PageActions)) as PageActions;

            pageEvaluator
            .Requires()
            .GoToUrl("nuget.org/packages")
            .IsOnPage()
            .Fulfill();

            pageEvaluator.HasErrosOnPage();

            Assert.IsTrue(pageEvaluator.ErrorMessages[0].Contains("packages"));
            Assert.IsTrue(pageEvaluator.ErrorMessages[1].Contains("Swagger"));
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageEvaluator")]
        public void Should_Evaluate_Error_Messages_And_Throws_Selector_Exception()
        {
            var pageEvaluator = GetService(typeof(Page)) as Page;

            pageEvaluator
            .Requires()
            .GoToUrl("nuget.org/packages")
            .IsOnPage()
            .Fulfill();

            Assert.ThrowsException<SelectorException>(() => pageEvaluator.HasErrosOnPage());
        }
    }
}