using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NugetPackageFinder.Model;
using NugetPackageFinder.Pages.NugetPages;

namespace NugetPackageFinder
{
    public class Worker : BackgroundService
    {
        private PackagesPage _packagesPage { get; set; }
        private POMSeleniumPage _pomSeleniumPage { get; set; }
        
        public Worker(PackagesPage packagesPage, POMSeleniumPage pomSeleniumPage)
        {
            _packagesPage = packagesPage;
            _pomSeleniumPage = pomSeleniumPage;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Searching awesome packages on www.nuget.org
            // with Fluent Interface
            _packagesPage
            .Requires() // Design by Contract
            .GoToUrl("nuget.org/packages") // Go to page itself
            .IsOnPage() // Uses "PageReference" selector for this evaluation
            .Fulfill(); // Contract evaluation ensures that you are testing within the minimum conditions

            var foundPackage = _packagesPage
            .SetPageValues("pom selenium") // Uses "SetFields" selectors for search input - field index can be supplied
            .PerformClick() // Uses "PageButtons" selectors to perform click on configured element - button index can be supplied
            .HasTableResults(); // Uses "Table" selector for this validation

            if (!foundPackage)
                return;

            _packagesPage.PerformClick();// Clicks on POM.Selenium element based on "PageButtons"

            // The contract requires being on the right page (POM.Selenium package page)
            _pomSeleniumPage
            .Requires()
            .IsOnPage()
            .Fulfill();
            
            //Fill the model with values captured from page - based on "GetFields" selector
            _pomSeleniumPage.GetPageValues<PackageStatistics>();
            var packageModel = _pomSeleniumPage
            .PageValues as PackageStatistics; // Cast dynamic object to model type
            
            // Show results captured from POM.Selenium page
            Console.WriteLine("Total Downloads: "+packageModel.TotalDownloads);
            Console.WriteLine("of Current Version: " +packageModel.DownloadsCurrentVersion);
            Console.WriteLine("Download Per Day: "+packageModel.DownloadPerDay);
            
            // Ends navigation with a more flexible method
            _pomSeleniumPage.Execute(driver => driver?.Dispose());
        }
    }
}
