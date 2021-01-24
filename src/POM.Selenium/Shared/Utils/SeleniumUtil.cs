using System.IO;
using System.Drawing;
using OpenQA.Selenium;
using static System.TimeSpan;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace POM.Selenium.Shared.Utils
{
    public static class SeleniumUtil
    {
        public static IWebElement WaitElement(IWebDriver driver, By by, int seconds = 10)
        {
            try
            {
                var wait = new WebDriverWait(driver, FromSeconds(seconds));
                var element = wait.Until(d => d.FindElement(by));

                return element;
            }
            catch (System.Exception error)
            {
                throw error;
            }
        }

        public static IEnumerable<IWebElement> WaitElements(IWebDriver driver, By by, int seconds = 10)
        {
            try
            {
                var wait = new WebDriverWait(driver, FromSeconds(seconds));
                var elements = wait.Until(d => d.FindElements(by));

                return elements;
            }
            catch (System.Exception error)
            {
                throw error;
            }
        }

        public static Bitmap ToBitmap(IWebElement webElement)
        {
            byte[] byteArray = (webElement.GetWebDriver() as ITakesScreenshot).GetScreenshot().AsByteArray;
            Bitmap screenshot = new Bitmap(new MemoryStream(byteArray));

            Rectangle croppedImage = new Rectangle(webElement.Location.X, webElement.Location.Y, webElement.Size.Width, webElement.Size.Height);
            return screenshot.Clone(croppedImage, screenshot.PixelFormat);
        }

        public static byte[] ToBitmap(IWebElement webElement, bool flag) => (webElement.GetWebDriver() as ITakesScreenshot).GetScreenshot().AsByteArray;
        
        private static IWebDriver GetWebDriver(this IWebElement webElement) => (webElement as IWrapsDriver).WrappedDriver;
    }
}