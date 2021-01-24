using System;
using OpenQA.Selenium;
using System.Collections.Generic;
using POM.Selenium.Contracts;

namespace POM.Selenium.Entities.Pages
{
    public abstract partial class Page<TPage> : IPageEntity
        where TPage: class
    {
        public Guid ID { get; private set; }
        protected readonly IWebDriver _driver;
        protected readonly Selector _selector;
        public int ClickIndex { get; private set; }
        public dynamic PageValues { get; private set; }
        public List<string[]> TableValues { get; private set; }
        public Dictionary<string, IEnumerable<IWebElement>> Instances { get; private set; }
        public List<IWebElement> TableInstances { get; private set; }
        public List<string> ErrorMessages { get; private set; }

        protected Page(IWebDriver driver, Selector selector)
        {
            _driver = driver;
            _selector = selector;
            ClickIndex =  default;
            ErrorMessages = new List<string>();
            Instances = new Dictionary<string, IEnumerable<IWebElement>>();
            ID = Guid.NewGuid();
        }
        public Contract<TPage> Requires() => new Contract<TPage>(this);
    }
}