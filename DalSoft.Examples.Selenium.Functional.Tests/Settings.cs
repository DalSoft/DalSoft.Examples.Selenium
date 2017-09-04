using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace DalSoft.Examples.Selenium.Functional.Tests
{

    public class Settings<T> where T : class, new()
    {
        private static T _settings;

        public static T GetSettings()
        {
            if (_settings != null)
                return _settings;
            
            _settings = GetConfig().GetSettings<T>();

            return _settings;
        }

        private static IConfigurationRoot GetConfig()
        {
            var binFolder = Directory.GetCurrentDirectory();

            return new ConfigurationBuilder()
                .SetBasePath(binFolder)
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
