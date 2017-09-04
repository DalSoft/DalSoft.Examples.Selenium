using DalSoft.Examples.Selenium.Functional.Tests.PageObjects.Components;
using OpenQA.Selenium;

namespace DalSoft.Examples.Selenium.Functional.Tests.PageObjects
{
    public abstract class GoogleBase
    {
        protected readonly IWebDriver WebDriver;
        
        public NavBar NavBar { get; set; }
        public Footer Footer { get; set; }

        protected GoogleBase(IWebDriver webDriver) : this(webDriver, null) { }

        protected GoogleBase(IWebDriver webDriver, string url)
        {
            WebDriver = webDriver;
            WebDriver.Manage().Window.Maximize(); //Bug in phantomjs https://github.com/ariya/phantomjs/issues/11637 if you don't do this it sets display false on elements out view which means it errors on click etc
            
            if (url != null)
                WebDriver.Navigate().GoToUrl(url);

            NavBar = new NavBar(WebDriver);
            Footer = new Footer(WebDriver);
        }
    }
}
