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

namespace StockportGovUK.AspNetCore.Attributes.TokenAuthentication
{
    public class TokenAuthenticationAttribute : ActionFilterAttribute
    {
        private static string defaultConfigurationSection = "TokenAuthentication";
        private string Key { get; set; }

        public TokenAuthenticationAttribute(IConfiguration configuration)
        {
            var TokenAuthenticationSection = configuration.GetSection(defaultConfigurationSection);
            var tokenAuthenticationConfiguration = new TokenAuthenticationConfiguration();            
            if (TokenAuthenticationSection.AsEnumerable().Any())
            {
                TokenAuthenticationSection.Bind(tokenAuthenticationConfiguration);
            }

            Key = tokenAuthenticationConfiguration.Key;
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            // todo : Get auth token from configuration
            try
            {
                var querystring = actionContext.HttpContext.Request.Query.FirstOrDefault(a => a.Key == "api_key");

                var authToken = querystring.Value.First();
                if(authToken != default(StringValues))
                {
                    authToken = GetTokenFromRequestHeaders(actionContext.HttpContext.Request);
                }

                var authenticator = new TokenAuthenticator(Key);
                var authenticationResult = authenticator.Authenticate(authToken);
                if (authenticationResult.IsAuthenticated)
                {
                    return;
                }

                actionContext.Result = new UnauthorizedObjectResult(authenticationResult.Reason);
            }
            catch (Exception)
            {
                actionContext.Result = new BadRequestObjectResult("Your request could not be processed"){ StatusCode = 501 };
            }
        }

        private static string GetTokenFromRequestHeaders(HttpRequest request)
        {
            if(request.Headers.TryGetValue("Authorization", out StringValues authToken))
            {
                return authToken.Last();
            }
            
            return string.Empty;
        }
    }
}
