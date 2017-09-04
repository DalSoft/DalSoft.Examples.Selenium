

namespace DalSoft.Examples.Selenium.Functional.Tests
{
    public class TestSettings : Settings<TestSettings>
    {
        public string WebDriver { get; set; }
        public short DriverWaitInSeconds { get; set; }

        public string GoogleUrl { get; set; }
    }
}
