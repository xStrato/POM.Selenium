using System.Linq;
using OpenQA.Selenium;
using static System.Convert;
using System.Collections.Generic;
using POM.Selenium.Shared.Utils;
using System;
using POM.Selenium.Exceptions.Selector;

namespace POM.Selenium.Entities.Pages
{
    public abstract partial class Page<TPage>
    {
        public void Execute(Action<IWebDriver> command) => command(_driver);
        public void Execute(Action<IWebDriver, ReadOnlySelector> command) => command(_driver, Converter.Selector<ReadOnlySelector>(_selector));
        public TResult Execute<TResult>(Func<IWebDriver, TResult> command) => command(_driver);
        public TResult Execute<TResult>(Func<IWebDriver, ReadOnlySelector, TResult> command) => 
        (
            command(_driver, Converter.Selector<ReadOnlySelector>(_selector))
        );
        
        public virtual TPage WaitLoad(int seconds=2)
        {
            if(string.IsNullOrEmpty(_selector.LoadElement)) 
                throw new SelectorException("The selector for 'LoadElement' property hasn't been configured.");
            try
            {
                while(SeleniumUtil.WaitElement(_driver, GetBySelector(_selector.LoadElement), 1).Displayed)
                    System.Threading.Thread.Sleep(1000*seconds);

            } catch {}
            
            return ChangeType(this, typeof(TPage)) as TPage;
        }

        /// <summary> </summary>
        /// <param name="frames">Recebe um params de inteiros onde "-1" significa um Switch para "DefaultContent" 
        ///que funciona basicamente como "reset" da navegação pelos frames</param>
        public TPage SwitchFrames(params int[] frames)
        {
            try
            {
                foreach (var frame in frames)
                {
                    if(frame is -1) 
                    { 
                        _driver.SwitchTo().DefaultContent(); 
                        continue; 
                    }
                    _driver.SwitchTo().Frame(frame);
                    System.Threading.Thread.Sleep(500);
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            return ChangeType(this, typeof(TPage)) as TPage;
        }

        private void HandleInputType(string selector, string value)
        {
            var element = _driver.FindElement(GetBySelector(selector));

            if (selector.StartsWith("input") || selector.StartsWith("textarea"))
                 (_driver as IJavaScriptExecutor).ExecuteScript($@"arguments[0].value='{value}';", element);
                 
            else if (selector.StartsWith("select"))
                element.SendKeys(value);

            else
                (_driver as IJavaScriptExecutor).ExecuteScript($@"arguments[0].textContent='{value}';", element);
        }

        public By GetBySelector(string selector)
        {
            switch(selector[0])
            {
                case '/': return By.XPath(selector);
                default: return By.CssSelector(selector);
            };
        }
        
        private List<string> RetrieveValuesFromWebElement(IWebElement element, string innerSelector="td")
        {
            try
            {
                return element
                .FindElements(GetBySelector(innerSelector))
                .ToList()
                .Select(el => el.Text)
                .ToList();
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        private List<string[]> RetrieveValuesFromWebElement(IEnumerable<IWebElement> elements, string innerSelector)
        {
            try
            {
                var list = new List<string[]>();
                foreach (var element in elements)
                {
                    var item = element.FindElements(GetBySelector(innerSelector)).Select(el => el.Text).ToArray();
                    list.Add(item);
                }
                return list;
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}