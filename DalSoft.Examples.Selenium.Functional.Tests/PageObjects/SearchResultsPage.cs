using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace DalSoft.Examples.Selenium.Functional.Tests.PageObjects
{
    public class SearchResultsPage : GoogleBase
    {
        public SearchResultsPage(IWebDriver webDriver) : base(webDriver)
        {
            WebDriver.Wait().For(_ => _.FindElement(By.Id("resultStats")));
            PageFactory.InitElements(WebDriver, this);
        }

        [FindsBy(How = How.Id, Using = "resultStats")]
        public IWebElement ResultStats { get; set; }

        [FindsBy(How = How.Id, Using = "ires")]
        public IWebElement Results { get; set; }
    }
}