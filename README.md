# DalSoft.Examples.Selenium.Functional.Tests

This is an example .NET Core (.Net Framework 4.6) project that shows how to setup Selenium in a frictionless way. The project simply tests google.co.uk, but it shows how to use the PageObject Factory properly in C#.

To setup Selenium it uses the following NuGet packages:

* Selenium.WebDriver

* Selenium.Support

* Selenium.Chrome.WebDriver

* Selenium.Firefox.WebDriver

* Selenium.InternetExplorer.WebDriver

* Selenium.PhantomJS.WebDriver

All the Web Drivers packages are by [jbaranda](https://github.com/jbaranda/nupkg-selenium-webdrivers) which are minimal install. These packages copy the correct files into your bin for you. The beauty of this is your CI server can just pull the source build and run, no more complex build scripts or commiting binaries to source control.

Below explains how the project is setup - it's lightweight no more than an abstract class, config and extensions methods.

## Components 

Components are shared PageObject’s that make up the structure of the Web UI under test. You would add anything you want to interact with that is shared across pages, navigation, footer etc.

This is achieved by creating an abstract class that represents your Web UI. The abstract class just has properties the represent each component. In the constructor we take a WebDriver and optional url, then we just assign each of the components and initiate by passing the  WebDriver.

```cs
protected GoogleBase(IWebDriver webDriver, string url)
{
   WebDriver = webDriver;
   WebDriver.Manage().Window.Maximize(); 
            
   if (url != null)
     WebDriver.Navigate().GoToUrl(url);

  NavBar = new NavBar(WebDriver);
  Footer = new Footer(WebDriver);
}
```
 

> Usually you should favour composition over inheritance, but it makes sense in this case if you think about the structure of a page. 

## PageObject 

The PageObject is just a class that represents a page. You need to inherit from the abstract class containing your components, then using the FindsBy attribute add the properties representing the elements in the page, you will also add methods that interact with the page for example Login().

## PageObjectFactory

The PageObjectFactory is just the term used for using the FindsBy attribute and the PageFactory.InitElements method to populate your PageObject. We do this in the constructor first we optionally load a url via the base constructor, then use the Wait() extension method to ensure the element is available before calling PageFactory.InitElements - see extensions.  

```cs
public class SearchPage : GoogleBase
{
	private readonly IWebDriver _webDriver;

	public SearchPage(IWebDriver webDriver) : base(webDriver, "https://www.google.co.uk/")
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
```

> Notice in this example we don't pass a url to the base constructor, this beacuse it's returned by SearchPage Search() Method.

```cs
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
```

> **Important** make sure you always call Wait() before PageFactory.InitElements otherwise you will get exceptions randomly because the Elements are not DOM ready after loading the page.

## Extensions

I’ve written extension methods that abstract away some of the friction of when an element is available to interact with. Selenium does have good support to ensure an element is available, however it is cumbersome hence the need for a fluent extension.

Your notice in our PageObject a call to Wait() just before PageFactory.InitElements:

```cs
webDriver.Wait().For(_ => _.FindElement(By.Id("resultStats")));
```

This extension method is just saying Wait() for the element with the id "resultStats" to be loaded before doing anything else (which is why it's the ctor).


There is also a Use() method for you to use in your PageObject methods, this is a good way to ensure an element is available and then use it in one line. This extension is useful when testing Single Page Apps when the element still might not be available even when the page is loaded. 

```cs
WebDriver.Wait().For(_ => _.FindElement(By.Id("MyReactButton"))).And().Use().Click();
```

this.CreateWebDriver() Creates the WebDriver based what set in appsettings.json - see settings.


## Settings

Settings uses DalSoft.Configuration making it trivial to use in your tests. Add your setting to appsettings.json and TestSettings.cs, then Just call TestSettings.GetSettings() and select whatever property you need.

I've made it trivial to switch between drivers, just change WebDriver settings in appsettings.json, for example 
* OpenQA.Selenium.PhantomJS.PhantomJSDriver
* OpenQA.Selenium.IE.InternetExplorerDriver
* OpenQA.Selenium.Chrome.ChromeDriver
* OpenQA.Selenium.Firefox.FirefoxDriver

## Tests

So we have done all the work with the PageObject to get to here - to test all you need to do is interact with your PageObject and Assert. If your think using the PageObject is a lot of hassle think again... If the Web UI your testing changes all you have to do is change the PageObject not the a tests. Also we are enforcing good separation of concerns between interacting with the Selenium and the tests.

```cs 
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
```

## CI Server

Because we have used jbaranda’s Web Drivers, just pull the source, build and run the tests in the normal way no special build steps (Of course your need the relevant browser installed on the server).