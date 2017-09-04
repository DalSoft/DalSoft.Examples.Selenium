using System;
using DalSoft.Examples.Selenium.Functional.Tests.PageObjects;
using OpenQA.Selenium;
using Xunit;

namespace DalSoft.Examples.Selenium.Functional.Tests
{
    public class GoogleTests : IDisposable
    {
      private readonly IWebDriver _webDriver;
     
       public GoogleTests()
       {
           _webDriver = this.CreateWebDriver();
       }
        
        [Fact]
        public void Search_SearchForDalSoft_DalSoftRestClientShouldBeInTheTop10SearchResults()
        {
            var searchResults = new SearchPage(_webDriver)
                .Search("DalSoft");

            Assert.Contains("DalSoft.RestClient", searchResults.Results.Text);   
        }

        public void Dispose()
        {
            this._webDriver.Quit();
        }
    }
}
