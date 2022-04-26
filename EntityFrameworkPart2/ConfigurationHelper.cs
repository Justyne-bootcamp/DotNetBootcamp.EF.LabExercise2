using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace EntityFrameworkPart2
{
    public class ConfigurationHelper
    {
        private static ConfigurationHelper INSTANCE;
        public IConfigurationRoot Configuration { get; set; }
        private ConfigurationHelper()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>()
                .Build();
        }
        public static ConfigurationHelper Instance()
        {
            if(INSTANCE == null)
            {
                INSTANCE = new ConfigurationHelper();
            }
            return INSTANCE;
        }

        public T GetProperty<T>(string propertyName)
        {
            return Configuration.GetValue<T>(propertyName);

        }
    }
}
