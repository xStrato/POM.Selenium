using System.Linq;

namespace POM.Selenium.Entities
{
    public class Selector: BaseSelector
    {
        public string PageReference { get; set; }
        public string[] SetFields { get; set; }
        public string[] GetFields { get; set; }
        public string[] PageButtons { get; set; }
        public string Table { get; set; }
        public string[] TableFields { get; set; }
        public string LoadElement { get; set; }
        public string[] ErrorElements { get; set; }

        public Selector() { }
        public Selector(string pageReference, string[] setFields, string[] getFields, string[] pageButtons, string table, string[] tableFields, string loadElement, string[] errorElements)
        {
            PageReference = pageReference;
            SetFields = setFields;
            GetFields = getFields;
            PageButtons = pageButtons;
            Table = table;
            TableFields = tableFields;
            LoadElement = loadElement;
            ErrorElements = errorElements;
        }
        public override bool ValidateSelectors() => 
        (
            !string.IsNullOrEmpty(PageReference) &&
            !string.IsNullOrEmpty(Table) &&
            !string.IsNullOrEmpty(LoadElement) &&
            SetFields.Any() &&
            GetFields.Any() &&
            PageButtons.Any() &&
            TableFields.Any() &&
            ErrorElements.Any()
        );
    }
}