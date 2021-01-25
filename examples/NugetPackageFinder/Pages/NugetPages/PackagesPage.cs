
using OpenQA.Selenium;
using POM.Selenium.Entities;
using POM.Selenium.Entities.Pages;

namespace NugetPackageFinder.Pages.NugetPages
{
    public class PackagesPage : Page<PackagesPage>
    {
        public PackagesPage(IWebDriver driver, Selector selector) : base(driver, selector){}
    }
}