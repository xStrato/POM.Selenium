using System.Linq;
using OpenQA.Selenium;
using static System.Convert;
using POM.Selenium.Shared.Utils;
using POM.Selenium.Exceptions.Selector;
using POM.Selenium.Exceptions.Page;

namespace POM.Selenium.Entities.Pages
{
    public abstract partial class Page<TPage>
    {
        public virtual TPage PerformClick(int buttonIndex, int seconds=5)
        {
            if (buttonIndex >= _selector.PageButtons.Length)
                throw new SelectorException($"The button index ({buttonIndex}) is greater than the number of 'PageButtons' selectors: {_selector.PageButtons.Length}.");

            if(SeleniumUtil.WaitElement(_driver, GetBySelector(_selector.PageButtons[buttonIndex]), seconds).Displayed)
            {
                var element = _driver.FindElement(GetBySelector(_selector.PageButtons[buttonIndex]));
                
                try{ element.Click(); } 
                catch { (_driver as IJavaScriptExecutor).ExecuteScript($@"arguments[0].click()", element); }

                System.Threading.Thread.Sleep(500);
            }
            return ChangeType(this, typeof(TPage)) as TPage;
        }

        public virtual TPage PerformClick(int seconds=5)
        {
            if (ClickIndex >= _selector.PageButtons.Length)
                throw new SelectorException($"The actual index of property 'ClickIndex' ({ClickIndex}) is greater than the number of 'PageButtons' selectors: {_selector.PageButtons.Length}.");

            if(SeleniumUtil.WaitElement(_driver, GetBySelector(_selector.PageButtons[ClickIndex]), seconds).Displayed)
            {
                var element = _driver.FindElement(GetBySelector(_selector.PageButtons[ClickIndex]));

                try{ element.Click(); }
                catch { (_driver as IJavaScriptExecutor).ExecuteScript($@"arguments[0].click()", element); }

                ClickIndex++;
                System.Threading.Thread.Sleep(500);
            }
            return ChangeType(this, typeof(TPage)) as TPage;
        }

        public virtual TPage PerformInstanceClick(int index=0, int seconds=1, int loopIteractions=1, string selector="td")
        {
            if(TableInstances is null || TableInstances.Any(el => el is null))
                throw new PageException("Can't perform Instance Click due to the property 'TableInstances' being null or empty. First call 'GetTableInstances' before trying to retrieve it's values.");

            loopIteractions = loopIteractions == -1 
                ? TableInstances.Count 
                : loopIteractions;

            for(var i = 0; i < loopIteractions; i++)
            {
                var instance = TableInstances[i].FindElements(GetBySelector(selector))[index];

                (_driver as IJavaScriptExecutor).ExecuteScript("arguments[0].scrollIntoView(true);", instance);
                instance.Click();
                
                System.Threading.Thread.Sleep(1000 * seconds);
            }
            return ChangeType(this, typeof(TPage)) as TPage;
        }
    }
}