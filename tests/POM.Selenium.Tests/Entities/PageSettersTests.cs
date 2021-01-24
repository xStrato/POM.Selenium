using Microsoft.VisualStudio.TestTools.UnitTesting;
using POM.Selenium.Tests.Pages.TestsPages;
using POM.Selenium.Tests.Config;
using System;
using OpenQA.Selenium;
using POM.Selenium.Exceptions.Selector;

namespace POM.Selenium.Tests.Entities
{
    [TestClass]
    public class PageSettersTests
    {
        private delegate object ServiceProvider(Type type);
        private static ServiceProvider GetService {get; set; }
        public PageSettersTests()
        {
            TesterHostBuilder.Init();
            GetService = new ServiceProvider(TesterHostBuilder.Host.Services.GetService);
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageSetters")]
        public void Should_Set_Page_Values_With_Multiple_Params()
        {
            var pageSetters = GetService(typeof(PageActions)) as PageActions;

            pageSetters
            .Requires()
            .GoToUrl("formulario.detran.sp.gov.br/Planilha.aspx")
            .Fulfill();

            var pageValues = new[]
            {
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString()
            };

            var valitation = pageSetters
            .SetPageValues(pageValues)
            .Execute((d, s) => new
            {
                MyName = d.FindElement(By.CssSelector(s.SetFields[0])).GetProperty("value"),
                MyAddress = d.FindElement(By.CssSelector(s.SetFields[1])).GetProperty("value"),
                MyNumber = d.FindElement(By.CssSelector(s.SetFields[2])).GetProperty("value"),
                MyNeighbor = d.FindElement(By.CssSelector(s.SetFields[3])).GetProperty("value")
            });

            Assert.AreEqual(pageValues[0], valitation.MyName);
            Assert.AreEqual(pageValues[1], valitation.MyAddress);
            Assert.AreEqual(pageValues[2], valitation.MyNumber);
            Assert.AreEqual(pageValues[3], valitation.MyNeighbor);
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageSetters")]
        public void Should_Set_Page_Values_With_Specific_Locators()
        {
            var pageSetters = GetService(typeof(PageActions)) as PageActions;

            pageSetters
            .Requires()
            .GoToUrl("formulario.detran.sp.gov.br/Planilha.aspx")
            .Fulfill();

            var pageValues = new[]
            {
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString()
            };

            var valitation = pageSetters
            .SetPageValues(value:pageValues[0], selectorIndex:0)
            .SetPageValues(value:pageValues[1], selectorIndex:1)
            .SetPageValues(value:pageValues[2], selectorIndex:2)
            .SetPageValues(value:pageValues[3], selectorIndex:3)
            .Execute((d, s) => new
            {
                MyName = d.FindElement(By.CssSelector(s.SetFields[0])).GetProperty("value"),
                MyAddress = d.FindElement(By.CssSelector(s.SetFields[1])).GetProperty("value"),
                MyNumber = d.FindElement(By.CssSelector(s.SetFields[2])).GetProperty("value"),
                MyNeighbor = d.FindElement(By.CssSelector(s.SetFields[3])).GetProperty("value")
            });

            Assert.AreEqual(pageValues[0], valitation.MyName);
            Assert.AreEqual(pageValues[1], valitation.MyAddress);
            Assert.AreEqual(pageValues[2], valitation.MyNumber);
            Assert.AreEqual(pageValues[3], valitation.MyNeighbor);
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageSetters")]
        public void Should_Set_Page_Values_On_Element_Type_Textarea()
        {
            var pageSetters = GetService(typeof(PageActions)) as PageActions;

            pageSetters
            .Requires()
            .GoToUrl("formulario.detran.sp.gov.br/Planilha.aspx")
            .Fulfill();

            var pageValue = Guid.NewGuid().ToString();

            var textareaValue = pageSetters
            .SetPageValues(value:pageValue, selectorIndex:4)
            .Execute((d, s) => d.FindElement(By.CssSelector(s.SetFields[4])).GetProperty("value"));

            Assert.AreEqual(pageValue, textareaValue);
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageSetters")]
        public void Should_Set_Page_Values_On_Element_Type_Select()
        {
            var pageSetters = GetService(typeof(PageActions)) as PageActions;

            pageSetters
            .Requires()
            .GoToUrl("formulario.detran.sp.gov.br/Planilha.aspx")
            .Fulfill();

            var valitation = pageSetters
            .SetPageValues(value:"Diesel", selectorIndex:5)
            .SetPageValues(selectOption:3, selectorIndex:6)
            .Execute((d, s) => new
            {
                Gas = (d as IJavaScriptExecutor).ExecuteScript($"return document.querySelector(\"{s.SetFields[5]}\").selectedOptions[0].text"),
                Color = (d as IJavaScriptExecutor).ExecuteScript($"return document.querySelector(\"{s.SetFields[6]}\").selectedOptions[0].text")
            });

            Assert.AreEqual("Diesel", valitation.Gas);
            Assert.AreEqual("Bege", valitation.Color);
        }


        [TestMethod]
        [TestCategory("Entities/Pages/PageSetters")]
        public void Should_Set_Page_Values_With_Multiple_Params_And_Throws_Selector_Exception()
        {
            var pageSetters = GetService(typeof(PageActions)) as PageActions;

            pageSetters
            .Requires()
            .GoToUrl("formulario.detran.sp.gov.br/Planilha.aspx")
            .Fulfill();

            var pageValues = new[]
            {
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString()
            };

            Assert.ThrowsException<SelectorException>(() => pageSetters.SetPageValues(pageValues));
        }

        [TestMethod]
        [TestCategory("Entities/Pages/PageSetters")]
        public void Should_Set_Page_Values_With_Specific_Locators_And_Throws_Selector_Exception()
        {
            var pageSetters = GetService(typeof(PageActions)) as PageActions;

            pageSetters
            .Requires()
            .GoToUrl("formulario.detran.sp.gov.br/Planilha.aspx")
            .Fulfill();

            Assert.ThrowsException<SelectorException>(() => pageSetters.SetPageValues(value:string.Empty, selectorIndex:10000));
        }
    }
}