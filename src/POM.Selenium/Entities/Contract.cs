using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using POM.Selenium.Entities.Pages;
using POM.Selenium.Exceptions.Contract;
using POM.Selenium.Exceptions.Selector;
using POM.Selenium.Shared.Utils;
using static System.Convert;

namespace POM.Selenium.Entities
{
    public class Contract<TPage> where TPage: class
    {
        private Page<TPage> _page;
        public bool IsFulfilled
        { 
            get
            {
                if(_unsuccessfulContracts is null)
                    throw new ContractException("The 'Fulfill' evaluation can't be performed before setting up some conditions.");
                
                return !_unsuccessfulContracts.Any();
            }
        }
        private IList<Exception> _unsuccessfulContracts { get;set; }
        public IReadOnlyCollection<Exception> UnsuccessfulContracts 
        { 
            get => new ReadOnlyCollection<Exception>(_unsuccessfulContracts); 
        }

        public Contract(Page<TPage> page) => _page = page;

        public Contract<TPage> GoToUrl(string BaseURL, int seconds=10)
        {
            try
            {
                if (_unsuccessfulContracts is null)
                    _unsuccessfulContracts = new List<Exception>();

                _page.Execute(d => d.Navigate().GoToUrl(BaseURL.StartsWith("http://") ? BaseURL : "http://"+BaseURL));
            }
            catch (Exception e)
            {
                _unsuccessfulContracts.Add(e);
            }
            return ChangeType(this, typeof(Contract<TPage>)) as Contract<TPage>;
        }

        public Contract<TPage> IsOnPage()
        {
            try
            {
                if (_unsuccessfulContracts is null)
                    _unsuccessfulContracts = new List<Exception>();

                _page.Execute((d, s) => SeleniumUtil.WaitElement(d, _page.GetBySelector(s.PageReference), 5));
            }
            catch (Exception e)
            {
                _unsuccessfulContracts.Add(e);
            }
            return ChangeType(this, typeof(Contract<TPage>)) as Contract<TPage>;
        }

        public Contract<TPage> ContainsExactText(params string[] textToSearch)
        {
            try
            {
                if (_unsuccessfulContracts is null)
                    _unsuccessfulContracts = new List<Exception>();

                if(textToSearch.Any(el => string.IsNullOrEmpty(el)))
                    throw new ContractException("The entries can't contain zeros or nulls");

                _page.Execute(d =>
                {
                    foreach (var text in textToSearch)
                        _ = d.FindElement(By.XPath($"//*[text()='{text}']")).Text;
                });
            }
            catch (Exception e)
            {
                _unsuccessfulContracts.Add(e);
            }
            return ChangeType(this, typeof(Contract<TPage>)) as Contract<TPage>;
        }

        public Contract<TPage> ContainsText(params string[] textToSearch)
        {
            try
            {
                if (_unsuccessfulContracts is null)
                    _unsuccessfulContracts = new List<Exception>();

                if(textToSearch.Any(el => string.IsNullOrEmpty(el)))
                    throw new ContractException("The entries can't contain zeros or nulls");

                _page.Execute(d => 
                {
                    foreach (var text in textToSearch)
                        _ = d.FindElement(By.XPath($"//*[contains(text(),'{text}')]")).Text;
                });
            }
            catch (Exception e)
            {
                _unsuccessfulContracts.Add(e);
            }
            return ChangeType(this, typeof(Contract<TPage>)) as Contract<TPage>;
        }

        public Contract<TPage> DeclareAllSelectors()
        {
            try
            {
                if (_unsuccessfulContracts is null)
                    _unsuccessfulContracts = new List<Exception>();

                if(!_page.Execute((_, s) => s.ValidateSelectors()))
                    throw new SelectorException("Not all selectors have been declared.");
            }
            catch (Exception e)
            {
                _unsuccessfulContracts.Add(e);
            }
            return ChangeType(this, typeof(Contract<TPage>)) as Contract<TPage>;
        }

        public Contract<TPage> DeclaredSelectorsExistsOnPage()
        {
            try
            {
                if (_unsuccessfulContracts is null)
                    _unsuccessfulContracts = new List<Exception>();

                _page.Execute((d, s) =>
                {
                    ExistsOnPage(d, new[]{ s.PageReference });
                    ExistsOnPage(d, new[]{ s.Table });
                    ExistsOnPage(d, new[]{ s.LoadElement });
                    ExistsOnPage(d, s.ErrorElements);
                    ExistsOnPage(d, s.GetFields);
                    ExistsOnPage(d, s.SetFields);
                    ExistsOnPage(d, s.PageButtons);
                    ExistsOnPage(d, s.TableFields);
                });
            }
            catch (Exception e)
            {
                _unsuccessfulContracts.Add(e);
            }
            return ChangeType(this, typeof(Contract<TPage>)) as Contract<TPage>;
        }

        private void ExistsOnPage(IWebDriver d, string[] selectors)
        {
            if (_unsuccessfulContracts is null)
                _unsuccessfulContracts = new List<Exception>();

            foreach (var selector in selectors)
            {
                if(string.IsNullOrEmpty(selector))
                    continue;

                d.FindElement(_page.GetBySelector(selector));
            }
        }
        
        public TPage Fulfill()
        {
            var page = _page;
            _page = default;
            
            if(_unsuccessfulContracts != null && _unsuccessfulContracts.Any())
                throw new UnsuccessfulContractException($"A total of {UnsuccessfulContracts.Count} requirement(s) haven't been fulfilled.", UnsuccessfulContracts);

            return page as TPage;
        }
    }
}