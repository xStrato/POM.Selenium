using OpenQA.Selenium;
using static System.Convert;
using POM.Selenium.Shared.Utils;
using POM.Selenium.Exceptions.Selector;

namespace POM.Selenium.Entities.Pages
{
    public abstract partial class Page<TPage>
    {
        public virtual TPage SetPageValues(params string[] pageValues)
        {
            try
            {
                if(pageValues.Length > _selector.SetFields.Length)
                    throw new SelectorException($"The entry values ({pageValues.Length}) must be less than or equal to the number of 'SetFields' selectors count: {_selector.SetFields.Length}");

                if (SeleniumUtil.WaitElement(_driver, GetBySelector(_selector.SetFields[0])).Displayed)
                    for (int i = 0; i < pageValues.Length; i++)
                        HandleInputType(_selector.SetFields[i], pageValues[i]);
            }
            catch (System.Exception)
            {
                throw;
            }
            return ChangeType(this, typeof(TPage)) as TPage;
        }

        public virtual TPage SetPageValues(int selectOption, int selectorIndex)
        {
            try
            {
                if(selectorIndex > _selector.SetFields.Length)
                    throw new SelectorException($"The informed selector index ({selectorIndex}) must be less than or equal to the number of 'SetFields' selectors count: {_selector.SetFields.Length}");

                if (SeleniumUtil.WaitElement(_driver, GetBySelector(_selector.SetFields[selectorIndex])).Displayed)
                {
                    var element = _driver.FindElement(GetBySelector(_selector.SetFields[selectorIndex]));
                    (_driver as IJavaScriptExecutor).ExecuteScript($@"arguments[0].selectedIndex={selectOption}", element);
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            return ChangeType(this, typeof(TPage)) as TPage;
        }
        
        public virtual TPage SetPageValues(string value, int selectorIndex)
        {
            try
            {
                if(selectorIndex > _selector.SetFields.Length)
                    throw new SelectorException($"The informed selector index ({selectorIndex}) must be less than or equal to the number of 'SetFields' selectors count: {_selector.SetFields.Length}");

                if (SeleniumUtil.WaitElement(_driver, GetBySelector(_selector.SetFields[selectorIndex])).Displayed)
                    HandleInputType(_selector.SetFields[selectorIndex], value);
            }
            catch (System.Exception)
            {
                throw;
            }
            return ChangeType(this, typeof(TPage)) as TPage;
        }
    }
}