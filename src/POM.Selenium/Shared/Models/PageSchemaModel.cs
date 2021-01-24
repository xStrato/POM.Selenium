using System.Collections.Generic;
using POM.Selenium.Entities;

namespace POM.Selenium.Shared.Models
{
    public class PageSchemaModel
    {
        public Dictionary<string, Dictionary<string, Selector>> Schema { get; }
        public PageSchemaModel(Dictionary<string, Dictionary<string, Selector>> schema) => Schema = schema;
    }
}