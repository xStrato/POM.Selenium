using System.Linq;
using POM.Selenium.Exceptions.Selector;
using POM.Selenium.Shared.Utils;

namespace POM.Selenium.Entities.Pages
{
    public abstract partial class Page<TPage>
    {
        public virtual bool HasTableResults(int seconds=5)
        {
            try
            {
                if (SeleniumUtil.WaitElement(_driver, GetBySelector(_selector.Table), seconds).Displayed)
                    return _driver.FindElements(GetBySelector(_selector.Table)).Any();
            }
            catch (System.Exception)
            {
                return false;
            }
            return false;
        }
        
        public virtual bool HasErrosOnPage()
        {
            if(_selector.ErrorElements is null || _selector.ErrorElements.Any(el => string.IsNullOrEmpty(el)))
                throw new SelectorException("The selector for 'ErrorElements' property hasn't been configured.");

            foreach (var errorSelector in _selector.ErrorElements)
            {
                try
                {
                    ErrorMessages.Add(_driver.FindElement(GetBySelector(errorSelector)).Text);
                }
                catch {continue;}
            }
            
            return ErrorMessages.Any();
        }
    }
}