using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace DalSoft.Examples.Selenium.Functional.Tests.PageObjects.Components
{
    public class Footer
    {
        public Footer(IWebDriver driver)
        {
            driver.Wait().For(_ => _.FindElement(By.LinkText("Privacy")));
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.LinkText, Using = "Privacy")]
        public IWebElement Privacy { get; set; }

        [FindsBy(How = How.LinkText, Using = "Terms")]
        public IWebElement Terms { get; set; }
    }
}
