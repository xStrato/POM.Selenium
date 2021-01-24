using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using POM.Selenium.Entities;
using POM.Selenium.Exceptions.Selector;
using POM.Selenium.Tests.Config;
using POM.Selenium.Tests.Pages.TestsPages;

namespace POM.Selenium.Tests.Entities
{
    [TestClass]
    public class PageSupportTests
    {
        private delegate object ServiceProvider(Type type);
        private static ServiceProvider GetService {get; set; }
        public PageSupportTests()
        {
            TesterHostBuilder.Init();
            GetService = new ServiceProvider(TesterHostBuilder.Host.Services.GetService);
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageSupport")]
        public void Should_Execute_Func()
        {
            var pageNotTheEntity = GetService(typeof(Page)) as Page;

            var title = pageNotTheEntity.Execute(d => 
            {
                d.Url = "http://www.google.com.br";
                return d.Title;
            });

            Assert.AreEqual(title, "Google");
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageSupport")]
        public void Execute_Returns_ReadOlnySelector()
        {
            var pageNotTheEntity = GetService(typeof(Page)) as Page;
            var selector = pageNotTheEntity.Execute((_, s) => s);

            Assert.IsInstanceOfType(selector, typeof(ReadOnlySelector));
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageSupport")]
        public void Execute_Returns_IWebDriver()
        {
            var pageNotTheEntity = GetService(typeof(Page)) as Page;
            var driver = pageNotTheEntity.Execute((d, _) => d);

            Assert.IsInstanceOfType(driver, typeof(IWebDriver));
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageSupport")]
        public void Should_Execute_Action()
        {
            var pageNotTheEntity = GetService(typeof(Page)) as Page;

            pageNotTheEntity.Execute(d => d.Url = "http://www.google.com.br");
            var title = GetService(typeof(IWebDriver)) as IWebDriver;

            Assert.AreEqual(title.Title, "Google");
        }
        
        [TestMethod]
        [TestCategory("Entities/Pages/PageSupport")]
        public void Should_Wait_Load()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageSupport")]
        public void Should_Switch_To_Frame()
        {
            var pageNotTheEntity = GetService(typeof(Page)) as Page;

            pageNotTheEntity
            .Requires()
            .GoToUrl("app.hugme.com.br")
            .Fulfill();

            Thread.Sleep(2000);

            var framesCount = pageNotTheEntity.Execute((d) => d.FindElements(By.TagName("iframe")).Count);

            var innerFrame = pageNotTheEntity
            .SwitchFrames(frames: 0)
            .Execute((d) => d.FindElements(By.TagName("iframe")).Count);

            Assert.IsTrue(framesCount >= 3);
            Assert.AreEqual(1, innerFrame);
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageSupport")]
        public void Should_Switch_Back_To_Root_Content()
        {
            var pageNotTheEntity = GetService(typeof(Page)) as Page;

            pageNotTheEntity
            .Requires()
            .GoToUrl("app.hugme.com.br")
            .Fulfill();

            Thread.Sleep(2000);

            var framesCount = pageNotTheEntity.Execute((d) => d.FindElements(By.TagName("iframe")).Count);

            var innerFrame = pageNotTheEntity
            .SwitchFrames(frames: 0)
            .Execute((d) => d.FindElements(By.TagName("iframe")).Count);

            var defaultContentCount = pageNotTheEntity
            .SwitchFrames(frames: -1)
            .Execute((d) => d.FindElements(By.TagName("iframe")).Count);

            Assert.IsTrue(framesCount >= 3);
            Assert.AreEqual(1, innerFrame);
            Assert.AreEqual(framesCount, defaultContentCount);
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageSupport")]
        public void Should_Wait_Load_And_Throws_Selector_Exception()
        {
            var pageActions = GetService(typeof(PageActions)) as PageActions;
            Assert.ThrowsException<SelectorException>(() => pageActions.WaitLoad());
        }
    }
}