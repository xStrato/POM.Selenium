using POM.Selenium.Contracts;

namespace POM.Selenium.Entities
{
    public abstract class BaseSelector: ISelector
    {
        public object this[string selector]
        {
            get => GetType().GetField(selector).GetValue(this);
            set => GetType().GetField(selector).SetValue(this, value);
        }
        
        public object this[int index]
        {
            get => GetType().GetFields()[index].GetValue(this);
            set => GetType().GetFields()[index].SetValue(this, value);
        }

        public abstract bool ValidateSelectors();
    }
}