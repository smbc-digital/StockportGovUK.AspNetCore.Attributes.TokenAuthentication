using Microsoft.Extensions.Configuration;

namespace StockportGovUK.AspNetCore.Attributes.TokenAuthentication.Tests
{
    public class ConfigurationLoadHelper
    {
        public static IConfiguration GetConfiguration()
        {            
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }   
    }
}
