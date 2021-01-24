namespace POM.Selenium.Shared.Models
{
    public static class TemplateModel
    {
        public static readonly string @Class = @"
using OpenQA.Selenium;
using POM.Selenium.Entities;
using POM.Selenium.Entities.Pages;

namespace {{project}}.{{pages}}.{{pageName}}
{
    public class {{className}} : Page<{{className}}>
    {
        public {{className}}(IWebDriver driver, Selector selector) : base(driver, selector){}
    }
}";
    }
}