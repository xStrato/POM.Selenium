using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POM.Selenium.Tests.Config;
using POM.Selenium.Tests.Pages.TestsPages;

namespace POM.Selenium.Tests.Entities
{
    [TestClass]
    public class PageTests
    {
       private delegate object ServiceProvider(Type type);
       private static ServiceProvider GetService {get; set; }
        public PageTests()
        {
            TesterHostBuilder.Init();
            GetService = new ServiceProvider(TesterHostBuilder.Host.Services.GetService);
        }

        [TestMethod]
        [TestCategory("Entities/Pages/Page")]
        public void Should_Return_A_Page_Instance()
        {
            var pageNotTheEntity = GetService(typeof(Page));
            Assert.IsInstanceOfType(pageNotTheEntity, typeof(Page));
        }

        [TestMethod]
        [TestCategory("Entities/Pages/Page")]
        public void Should_Return_A_Page_Identifier()
        {
            var pageNotTheEntity = GetService(typeof(Page)) as Page;
            Assert.IsInstanceOfType(pageNotTheEntity.ID, typeof(Guid));
        }
    }
}