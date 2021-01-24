using POM.Selenium.Contracts;
using System.Linq;

namespace POM.Selenium.Shared.Utils
{
    public static class Converter
    {
        public static TSelector Selector<TSelector>(object selector) 
            where TSelector : class, ISelector
        {
            var ctor = typeof(TSelector)
                .GetConstructors()
                .Single(c => c.GetParameters().Any());
            
            var modelProps = selector.GetType().GetProperties();
            var props = modelProps
                .Where(p => !p.Name.Equals("Item"))
                .Select(p => p.GetValue(selector)).ToArray();
            
            return ctor.Invoke(props) as TSelector;
        }
    }
}