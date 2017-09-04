using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace DalSoft.Examples.Selenium.Functional.Tests
{
    public static class SeleniumExtensions
    {
        public static int TimeOut => TestSettings.GetSettings().DriverWaitInSeconds;
        
        public static IWebElement GetParent(this IWebElement element)
        {
            return element.FindElement(By.XPath(".."));
        }

        public static WebDriverWait Wait(this IWebDriver driver)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(TimeOut));
        }

        public static WaitAnd<TResult> For<TResult>(this WebDriverWait driverWait, Func<IWebDriver, TResult> condition)
        {
            return new WaitAnd<TResult>(driverWait.Until(condition));
        }

        public static IWebDriver CreateWebDriver(this object o)
        {
            return (IWebDriver)Activator.CreateInstance("WebDriver", TestSettings.GetSettings().WebDriver).Unwrap();
        }
    }
    
    public class WaitAnd<TResult>
    {
        private readonly TResult _result;

        public WaitAnd(TResult result)
        {
            _result = result;
        }

        public WaitUse<TResult> And()
        {
            return new WaitUse<TResult>(_result);
        }
    }

    public class WaitUse<TResult>
    {
        private readonly TResult _result;

        public WaitUse(TResult result)
        {
            this._result = result;
        }

        public TResult Use()
        {
            return _result;
        }
    }
}