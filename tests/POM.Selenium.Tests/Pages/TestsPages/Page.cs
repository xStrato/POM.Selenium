using OpenQA.Selenium;
using POM.Selenium.Entities;
using POM.Selenium.Entities.Pages;

namespace POM.Selenium.Tests.Pages.TestsPages
{
    public class Page : Page<Page>
    {
        public Page(IWebDriver driver, Selector selector) : base(driver, selector){}
    }
}