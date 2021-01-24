using System;
using System.Collections.Generic;

namespace POM.Selenium.Exceptions.Contract
{
    public class UnsuccessfulContractException: AggregateException
    {
        public UnsuccessfulContractException(string message, IEnumerable<Exception> exceptions) : base(message, exceptions) { }
    }
}