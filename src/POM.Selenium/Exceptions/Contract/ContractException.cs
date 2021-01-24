using System;

namespace POM.Selenium.Exceptions.Contract
{
    public class ContractException: Exception
    {
        public ContractException(string message) : base(message) { }
    }
}