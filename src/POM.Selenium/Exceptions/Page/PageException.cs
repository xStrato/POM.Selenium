using System;

namespace POM.Selenium.Exceptions.Page
{
    public class PageException: Exception
    {
        public PageException(string message) : base(message) { }
    }
}