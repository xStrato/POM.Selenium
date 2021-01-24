using System.Linq;

namespace POM.Selenium.Entities
{
    public class ReadOnlySelector: BaseSelector
    {
        public readonly string PageReference;
        public readonly string[] SetFields;
        public readonly string[] GetFields;
        public readonly string[] PageButtons;
        public readonly string Table;
        public readonly string[] TableFields;
        public readonly string LoadElement;
        public readonly string[] ErrorElements;

        public ReadOnlySelector(string pageReference, string[] setFields, string[] getFields, string[] pageButtons, string table, string[] tableFields, string loadElement, string[] errorElements)
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