using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using POM.Selenium.Tests.Pages.TestsPages;
using static System.AppDomain;

namespace POM.Selenium.Tests.Config
{
    public static class TesterHostBuilder
    {
       public static IHost Host { get; private set; }
        public static void Init()
        {
            Host = new HostBuilder()
              .ConfigureServices((hostContext, services) =>
              {
                  services
                  .AddPomSelenium(autoGeneratePages:true)
                  .AddSingleton<IWebDriver>(_ =>
                  {
                     var options = new ChromeOptions();
                     options.AddArgument("--disable-notifications");
                     options.AddArgument("--disable-infobars");
                     options.AddArgument("--ignore-certificate-errors");
                     options.AddArgument("--disable-popup-blocking");
                     options.AddArgument("--disable-cache");
                     options.AddArgument("--disable-cached-picture-raster");
                     options.AddArgument("--disable-application-cache");
                     options.AddArgument("--disk-cache-size=0");
                     options.AddArgument("--headless");
                     
                     var service = ChromeDriverService.CreateDefaultService();
                     service.HideCommandPromptWindow = true;

                     var driver = new ChromeDriver(service, options);
                     CurrentDomain.ProcessExit += (s, e) => driver?.Dispose();
 
                     return driver;
                  });

                  services.AddSingleton(s => s.AddPage<Page>());
                  services.AddSingleton(s => s.AddPage<PageActions>());

              }).Build();
        }
    }
}