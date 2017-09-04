using Microsoft.Extensions.Configuration;

namespace DalSoft.Examples.Selenium.Functional.Tests
{
    public static class ConfigurationExtensions
    {
        public static TConfig GetSettings<TConfig>(this IConfigurationRoot configurationRoot) where TConfig : class, new()
        {
            var config = new TConfig();
            configurationRoot.GetSection(typeof(TConfig).Name).Bind(config);

            return config;
        }
    }
}