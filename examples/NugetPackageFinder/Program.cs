using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NugetPackageFinder.Pages.NugetPages;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NugetPackageFinder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services
                    .AddHostedService<Worker>()
                    .AddPomSelenium()
                    .AddSingleton<IWebDriver, ChromeDriver>()
                    .AddTransient(s => s.AddPage<PackagesPage>())
                    .AddTransient(s => s.AddPage<POMSeleniumPage>());
                });
    }
}
