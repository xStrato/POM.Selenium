using System.Linq;
using OpenQA.Selenium;
using static System.Convert;
using System.Collections.Generic;
using POM.Selenium.Shared.Utils;
using POM.Selenium.Exceptions.Page;
using POM.Selenium.Exceptions.Selector;
using System;

namespace POM.Selenium.Entities.Pages
{
    public abstract partial class Page<TPage>
    {
        public virtual TPage GetPageValues<TModel>(int propertyShift=0) where TModel: new()
        {
            var properties = typeof(TModel).GetProperties();

            if (properties.Length < _selector.GetFields.Length)
                throw new SelectorException($"The number of properties ({properties.Length}) in the model '{typeof(TModel).Name}' must be greater than or equal to the number of 'GetFields' selectors: {_selector.GetFields.Length}");

            try
            {
                if (SeleniumUtil.WaitElement(_driver, GetBySelector(_selector.GetFields[0])).Displayed)
                {
                    PageValues = new TModel();

                    for (int i = 0; i < _selector.GetFields.Length; i++)
                    {
                        var element = _driver.FindElement(GetBySelector(_selector.GetFields[i]));
                        var textValue = (_driver as IJavaScriptExecutor).ExecuteScript($"return arguments[0].value", element) as string;

                        if(textValue is null)
                            textValue = _driver.FindElement(GetBySelector(_selector.GetFields[i])).Text;

                        properties[i + propertyShift].SetValue(PageValues, textValue);
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            return ChangeType(this, typeof(TPage)) as TPage;
        }

        public virtual TPage GetPageValues(int seconds=5)
        {
            try
            {
                if (SeleniumUtil.WaitElement(_driver, GetBySelector(_selector.GetFields[0]), seconds).Displayed)
                {
                    PageValues = new List<string>();
                    for (int i = 0; i < _selector.GetFields.Length; i++)
                    {
                        var element = _driver.FindElement(GetBySelector(_selector.GetFields[i]));
                        var textValue = (_driver as IJavaScriptExecutor).ExecuteScript($"return arguments[0].value", element) as string;

                        if (textValue is null)
                            textValue = _driver.FindElement(GetBySelector(_selector.GetFields[i])).Text;

                        PageValues.Add(textValue);
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            return ChangeType(this, typeof(TPage)) as TPage;
        }

        public virtual TPage GetInstances(params string[] searchStrings)
        {
            foreach (var @string in searchStrings)
            {
                var elements = _driver.FindElements(GetBySelector($"//*[contains(text(), '{@string}')]"));

                if(elements.Any())
                    Instances.Add(@string, elements);
            }

            return ChangeType(this, typeof(TPage)) as TPage;
        }

        public virtual TPage GetInstance(params string[] searchStrings)
        {
            foreach (var @string in searchStrings)
            {
                var elements = _driver.FindElements(GetBySelector($"//*[text()='{@string}']"));

                if(elements.Any())
                    Instances.Add(@string, elements);
            }

            return ChangeType(this, typeof(TPage)) as TPage;
        }

        public virtual TPage GetTableInstances(params string[] tableValues)
        {
            if (tableValues is null || tableValues.All(el => string.IsNullOrEmpty(el))) 
                throw new ArgumentException("Can't perform the action due to the lack of entry values.");
                
            if (tableValues.Length > _selector.TableFields.Length) 
                throw new SelectorException($"The number of entry values ({tableValues.Length}) doesn't match the length of 'TableFields' selectors: {_selector.TableFields.Length}.");

            if (SeleniumUtil.WaitElement(_driver, GetBySelector(_selector.Table)).Displayed)
            {
                var table = _driver.FindElements(GetBySelector(_selector.Table));
                TableInstances = new List<IWebElement>();

                foreach (var element in table)
                {
                    var assertCount = 0;
                    for (int i = 0; i < tableValues.Length; i++)
                    {
                        if(element.FindElement(GetBySelector(_selector.TableFields[i])).Text.Equals(tableValues[i]))
                            assertCount++;
                    }

                    if (assertCount == tableValues.Length)
                        TableInstances.Add(element);
                }
            }
            return ChangeType(this, typeof(TPage)) as TPage;
        }

        public virtual TPage GetTableValues(string innerTableSelector)
        {
            if (TableInstances is null || TableInstances.Count <= 0)
                throw new PageException("There are no valid instances to perform the action. First call 'GetTableInstances' before trying to retrieve it's values.");

            TableValues = RetrieveValuesFromWebElement(TableInstances, innerTableSelector);
            return ChangeType(this, typeof(TPage)) as TPage;
        }
    }
}