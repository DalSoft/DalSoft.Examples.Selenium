using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace DalSoft.Examples.Selenium.Functional.Tests.PageObjects.Components
{
    public class NavBar
    {
        public NavBar(IWebDriver driver)
        {
            driver.Wait().For(_ => _.FindElement(By.LinkText("Sign in")));
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.LinkText, Using = "Sign in")]
        public IWebElement SignIn { get; set; }
    }
}
