
using OpenQA.Selenium;
using POM.Selenium.Entities;
using POM.Selenium.Entities.Pages;

namespace NugetPackageFinder.Pages.NugetPages
{
    public class POMSeleniumPage : Page<POMSeleniumPage>
    {
        public POMSeleniumPage(IWebDriver driver, Selector selector) : base(driver, selector){}
    }
}