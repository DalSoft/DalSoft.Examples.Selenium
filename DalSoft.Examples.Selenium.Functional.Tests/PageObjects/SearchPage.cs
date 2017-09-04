using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace DalSoft.Examples.Selenium.Functional.Tests.PageObjects
{
    public class SearchPage : GoogleBase
    {
        private readonly IWebDriver _webDriver;

        public SearchPage(IWebDriver webDriver) : base(webDriver, TestSettings.GetSettings().GoogleUrl)
        {
            _webDriver = webDriver;

            WebDriver.Wait().For(_ => _.FindElement(By.Name("btnK")));
            PageFactory.InitElements(WebDriver, this);
        }

        [FindsBy(How = How.Name, Using = "q")]
        private IWebElement SearchTextBox;

        [FindsBy(How = How.Name, Using = "btnK")]
        private IWebElement SearchButton;

        public SearchResultsPage Search(string q)
        {
            SearchTextBox.SendKeys(q);
            SearchTextBox.SendKeys(Keys.Return);

            return new SearchResultsPage(_webDriver);
        }
    }
}




