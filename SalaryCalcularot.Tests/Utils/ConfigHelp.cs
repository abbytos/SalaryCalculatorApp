using Microsoft.Extensions.Configuration;

namespace SalaryCalculatorTests.Utils
{
    public static class ConfigHelper
    {
        public static IConfiguration LoadConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();
        }

        public static T LoadConfig<T>(IConfiguration configuration, string sectionName) where T : class, new()
        {
            var configSection = configuration.GetSection(sectionName);
            if (configSection == null || !configSection.Exists())
            {
                throw new InvalidOperationException($"Configuration section '{sectionName}' is missing or empty.");
            }

            var config = configSection.Get<T>();
            if (config == null)
            {
                throw new InvalidOperationException($"Failed to bind configuration section '{sectionName}' to type '{typeof(T).Name}'.");
            }

            return config;
        }
    }
}
