using OpenQA.Selenium;
using POM.Selenium.Entities;
using POM.Selenium.Entities.Pages;

namespace POM.Selenium.Tests.Pages.TestsPages
{
    public class PageActions : Page<PageActions>
    {
        public PageActions(IWebDriver driver, Selector selector) : base(driver, selector){}
    }
}