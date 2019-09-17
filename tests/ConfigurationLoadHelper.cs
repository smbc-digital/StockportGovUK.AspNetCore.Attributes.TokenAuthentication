using Microsoft.Extensions.Configuration;

namespace StockportGovUK.AspNetCore.Attributes.TokenAuthentication.Tests
{
    public class ConfigurationLoadHelper
    {
        public static IConfiguration GetConfiguration()
        {           
            return  GetConfiguration("appsettings.json");
        }   

        public static IConfiguration GetConfiguration(string filename)
        {            
            return new ConfigurationBuilder()
                .AddJsonFile(filename, optional: false)
                .AddEnvironmentVariables()
                .Build();
        }   
    }
}
