![POM-Logo](https://github.com/xStrato/POM.Selenium/blob/main/media/pom-selenium.png)
# About
POM Selenium is a microframework and extensible wrapper for Selenium Webdriver which combines suitable patterns such as Page Object Models + Inversion of Control with Dependency Injection + Fluent Interface + Design by Contract, making web applications testing easy.
# Features
> **🙌No need to write code:** You'll find a set of generic methods that have been designed for general purposes.\
> **🗐 Auto-Generated Pages:** create a configuration file, configure pages selectors and have your class files ready.\
> **🛠️Extensible:** Is something missing? Expand the functionality to your need.
# Instalation
This package is also available on Nuget Packages: https://www.nuget.org/packages/POM.Selenium

**Nuget**
```
Install-Package POM.Selenium
```

**.NET CLI**
```
dotnet add package POM.Selenium
```
# Getting Started
## Setting up
The basic template for "Pages" setup is done through configuration file such as `appsettings.json` declaring a new section. Once it's done, the application will try to generate these pages based on the following convention:

> **appsettings.json** file:
``` json
{
  "Pages": {
    "NugetPages": {
      "HomePage": { ... },
      "SignInPage": { ... },
      "PackagesPage": { ... }
    },
    "AmazonPages": {
      "LoginPage": { ... },
      "CartPage": { ... },
      "CustomerServicePage": { ... }
    }
  }
}
```
> **Program.cs** file:
``` C#
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
                // Adding POM.Selenium with auto generated pages - requires the first execution
                services.AddPomSelenium(autoGeneratePages:true);
                // Setting up IWebDriver for this test
                services.AddSingleton<IWebDriver, ChromeDriver>();
                // Setting up www.nuget.org testing pages
                services.AddTransient(s => s.AddPage<HomePage>());
                services.AddTransient(s => s.AddPage<SignInPage>());
                services.AddTransient(s => s.AddPage<PackagesPage>());
                // Setting up www.amazon.com testing pages
                services.AddTransient(s => s.AddPage<LoginPage>());
                services.AddTransient(s => s.AddPage<CartPage>());
                services.AddTransient(s => s.AddPage<CustomerServicePage>());
            });
}
```
> **Folder** structure resulting:
```
[MyProject Folder]
├── bin
├── obj
├── Pages
│   ├── AmazonPages
│   │   ├── CartPage.cs
│   │   ├── CustomerServicePage.cs
│   │   └── LoginPage.cs
│   └── NugetPages
│       ├── HomePage.cs
│       ├── PackagesPage.cs
│       └── SignInPage.cs
├── appsettings.json
├── Program.cs
└── MyProject.csproj
```
## 📜 Convention
POM.Selenium follows the standards suggested by the `page object models` pattern, as shown below:
Item | Type | Convention | Example
------------ | ------------ | ------------ | -------------
Root | folder | `Pages` | A fixed keyword that works as an umbrella for all pages.
Sites | folder | `Site-name` + `Pages` suffix | AmazonPages, AirbnbPages, IkeaPages...
Pages | class | `Page-name` + `Page` suffix | LoginPage, ElectronicsPage, CheckoutPage, JobsPage...

## Choosing and configuring Page `selectors`
There are pre-defined selectors ready to be used at any time with the generic methods. 

Selector Name | Type | Description
------------ | ------------ | ------------
PageReference | string | A single element that represents the page itself (unique preferable).
PageButtons | string[] | A `set` of elements that represent buttons on the page.
SetFields | string[] | A `set` of elements that represent `values` to be `inserted` on the page.
GetFields | string[] | A `set` of elements that represent `values` to be `captured` from the page.
Table | string | A single element that represents the `rows` of a given table on the page.
TableFields | string[] | A `set` of elements that represent `columns` indexes to be captured from an existing `Table` selector.
ErrorElements | string[] | A `set` of elements that represent `erros` on the page.
LoadElement | string |  A single element that represents visual `loading` of the page.


Page `selectors` are only supported with **CssSelector** and **XPath** declaration, either option will be automatically detected at runtime.

Here is a real world example of how it can be used (get the entire code: [examples folder](https://pages.github.com/).):
> **appsettings.json** file:
``` json
{
  "Pages": {
  
      "NugetPages": {
      
          "PackagesPage": {
              "PageReference": "input#search",
              "Table": "#skippedToContent article.package",
              "PageButtons": ["button[title='Search for packages']", "[data-package-id='POM.Selenium']"],
              "SetFields": ["input#search"],
              "GetFields": null,
              "TableFields": null,
              "ErrorElements": null,
              "LoadElement": null
          },
          
          "POMSeleniumPage": {
              "PageReference": "//title[contains(text(),'POM.Selenium')]",
              "Table": null,
              "PageButtons": null,
              "SetFields": null,
              "GetFields": [
                "//aside[@aria-label='Package details info']//ul[2]//li[1]",
                "//aside[@aria-label='Package details info']//ul[2]//li[2]",
                "//aside[@aria-label='Package details info']//ul[2]//li[3]"],
              "TableFields": null,
              "ErrorElements": null,
              "LoadElement": null
          }
      }
  }
}
```
> **Program.cs** file:
``` C#
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
````
>  **Model/PackageStatistics.cs** file:
``` C#
public class PackageStatistics
{
    public string TotalDownloads { get; set; }
    public string DownloadsCurrentVersion { get; set; }
    public string DownloadPerDay { get; set; }
}
````
> **Worker.cs** file:
``` C#
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
````

# Documentation
W.I.P
