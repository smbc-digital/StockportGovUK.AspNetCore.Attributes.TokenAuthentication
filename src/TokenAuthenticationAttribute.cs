using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.DependencyInjection;

namespace StockportGovUK.AspNetCore.Attributes.TokenAuthentication
{
    public class TokenAuthenticationAttribute : ActionFilterAttribute
    {
        private static string defaultConfigurationSection = "TokenAuthentication";


        private string GetKey(ActionExecutingContext actionContext)
        {
            // This comes from here... https://www.devtrends.co.uk/blog/dependency-injection-in-action-filters-in-asp.net-core
            var configuration = actionContext.HttpContext.RequestServices.GetService<IConfiguration>();
            if (configuration == null)
            {
                throw new Exception("Could not load configuration");
            }

            var tokenAuthenticationSection = configuration.GetSection(defaultConfigurationSection);
            var tokenAuthenticationConfiguration = new TokenAuthenticationConfiguration();

            if (tokenAuthenticationSection == null)
            {
                throw new Exception("Token authentication is not configured");
            }
            else if (!tokenAuthenticationSection.AsEnumerable().Any())
            {
                throw new Exception("Token authentication is not configured");
            }
            
            tokenAuthenticationSection.Bind(tokenAuthenticationConfiguration);    
            return tokenAuthenticationConfiguration.Key;
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var key = GetKey(actionContext);
            try
            {
                var querystring = actionContext.HttpContext.Request.Query.FirstOrDefault(a => a.Key == "api_key");
                var authToken = querystring.Value.FirstOrDefault();

                if (string.IsNullOrEmpty(authToken))
                {
                    authToken = GetTokenFromRequestHeaders(actionContext.HttpContext.Request);
                }

                var authenticator = new TokenAuthenticator(key);
                var authenticationResult = authenticator.Authenticate(authToken);

                if (authenticationResult.IsAuthenticated)
                {
                    return;
                }

                // throw new Exception($"Env Key: {key} AuthToken: {authToken} - There was a problem.");
                actionContext.Result = new UnauthorizedObjectResult(authenticationResult.Reason);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                actionContext.Result = new BadRequestObjectResult("Your request could not be processed"){ StatusCode = 501 };
            }
        }

        private static string GetTokenFromRequestHeaders(HttpRequest request)
        {
            if (request.Headers.TryGetValue("Authorization", out StringValues authToken))
            {
                return authToken.Last()
                    .Split(" ")
                    .Last();
            }

            return string.Empty;
        }
    }
}
