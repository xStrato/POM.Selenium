using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POM.Selenium.Entities;
using POM.Selenium.Exceptions.Contract;
using POM.Selenium.Tests.Config;
using POM.Selenium.Tests.Pages.TestsPages;

namespace POM.Selenium.Tests.Entities
{
    [TestClass]
    public class ContractTests
    {
        private delegate object ServiceProvider(Type type);
        private static ServiceProvider GetService {get; set; }
        public ContractTests()
        {
            TesterHostBuilder.Init();
            GetService = new ServiceProvider(TesterHostBuilder.Host.Services.GetService);
        }

        [TestMethod]
        [TestCategory("Entities/Contact")]
        public void Should_Return_A_Contract()
        {
            var pageNotTheEntity = GetService(typeof(Page)) as Page;

            var contract = pageNotTheEntity.Requires();
            Assert.IsInstanceOfType(contract, typeof(Contract<Page>));
        }

        [TestMethod]
        [TestCategory("Entities/Contact")]
        public void Should_Return_A_Page()
        {
            var pageNotTheEntity = GetService(typeof(Page)) as Page;
            var contract = pageNotTheEntity.Requires();

            Assert.IsInstanceOfType(contract.Fulfill(), typeof(Page));
        }

        [TestMethod]
        [TestCategory("Entities/Contact")]
        public void Contract_Requires_Go_To_Url()
        {
            var pageNotTheEntity = GetService(typeof(Page)) as Page;

            pageNotTheEntity
            .Requires()
            .GoToUrl("google.com.br")
            .Fulfill();

            Assert.AreEqual("Google", pageNotTheEntity.Execute(d => d.Title));
        }

        [TestMethod]
        [TestCategory("Entities/Contact")]
        public void Contract_Requires_Is_On_Page_With_Go_To_Url()
        {
            var pageNotTheEntity = GetService(typeof(Page)) as Page;

            pageNotTheEntity
            .Requires()
            .GoToUrl("nuget.org")
            .IsOnPage()
            .Fulfill();

            Assert.AreEqual("NuGet Gallery | Home", pageNotTheEntity.Execute(d => d.Title));
        }

        [TestMethod]
        [TestCategory("Entities/Contact")]
        public void Contract_Requires_Contains_Text()
        {
            var pageNotTheEntity = GetService(typeof(Page)) as Page;
            try
            {
                pageNotTheEntity
                .Requires()
                .GoToUrl("nuget.org")
                .ContainsText("What is NuGet?", "Contact", "showing", "developers")
                .Fulfill();     
            }
            catch { Assert.IsTrue(false); }

            Assert.IsTrue(true);
        }

        [TestMethod]
        [TestCategory("Entities/Contact")]
        public void Contract_Requires_Contains_Exact_Text()
        {
            var pageNotTheEntity = GetService(typeof(Page)) as Page;
            try
            {
                pageNotTheEntity
                .Requires()
                .GoToUrl("nuget.org")
                .ContainsExactText(@"What is NuGet?")
                .Fulfill();
            }
            catch { Assert.IsTrue(false); }

            Assert.IsTrue(true);
        }

        [TestMethod]
        [TestCategory("Entities/Contact")]
        public void Contract_Requires_Contains_Exact_Text_To_Fail()
        {
            var pageNotTheEntity = GetService(typeof(Page)) as Page;
            var assert = false;

            try
            {
                pageNotTheEntity
                .Requires()
                .GoToUrl("nuget.org")
                .ContainsExactText(@"developers")
                .Fulfill();
            }
            catch { assert = true; }

            Assert.IsTrue(assert);
        }

        [TestMethod]
        [TestCategory("Entities/Contact")]
        public void Contract_Requires_Unsuccessful_Contracts_ReadOnlyCollection()
        {
            var pageNotTheEntity = GetService(typeof(Page)) as Page;
            var contract = pageNotTheEntity.Requires();

            contract
            .GoToUrl("nuget.org")
            .ContainsExactText(@"developers");

            Assert.IsInstanceOfType(contract.UnsuccessfulContracts, typeof(IReadOnlyCollection<Exception>));
        }

        [TestMethod]
        [TestCategory("Entities/Contact")]
        public void Contract_Requires_Declare_All_Selectors()
        {
            var pageNotTheEntity = GetService(typeof(Page)) as Page;
            var assert = true;
            
            try
            {
                pageNotTheEntity
                .Requires()
                .DeclareAllSelectors()
                .Fulfill();
            }
            catch { assert = false; }

            Assert.IsTrue(assert);
        }

        [TestMethod]
        [TestCategory("Entities/Contact")]
        public void Contract_Requires_Declared_Selectors_Exists_On_Page()
        {
            var pageNotTheEntity = GetService(typeof(Page)) as Page;
            var assert = true;
            
            try
            {
                pageNotTheEntity
                .Requires()
                .GoToUrl("nuget.org/packages")
                .DeclaredSelectorsExistsOnPage()
                .Fulfill();
            }
            catch { assert = false; }

            Assert.IsTrue(assert);
        }

        [TestMethod]
        [TestCategory("Entities/Contact")]
        public void Contract_Requires_Declared_Selectors_Exists_On_Page_Fail()
        {
            var pageNotTheEntity = GetService(typeof(Page)) as Page;
            var assert = false;
            
            try
            {
                pageNotTheEntity
                .Requires()
                .GoToUrl("nuget.org")
                .DeclaredSelectorsExistsOnPage()
                .Fulfill();
            }
            catch { assert = true; }

            Assert.IsTrue(assert);
        }

        [TestMethod]
        [TestCategory("Entities/Contact")]
        public void Contract_Requires_Throws_Contract_Exception()
        {
            var pageNotTheEntity = GetService(typeof(Page)) as Page;

            Assert.ThrowsException<ContractException>(() => pageNotTheEntity.Requires().IsFulfilled);
        }

        [TestMethod]
        [TestCategory("Entities/Contact")]
        public void Contract_Requires_Throws_Unsuccessful_Contract_Exception()
        {
            var pageNotTheEntity = GetService(typeof(Page)) as Page;

            Assert.ThrowsException<UnsuccessfulContractException>(() => 
            {
                pageNotTheEntity
                .Requires()
                .IsOnPage()
                .Fulfill();
            });
        }
    }
}