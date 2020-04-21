using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.DependencyInjection;

namespace StockportGovUK.AspNetCore.Attributes.TokenAuthentication
{
    public class TokenAuthenticationAttribute : ActionFilterAttribute
    {
        private static string defaultConfigurationSection = "TokenAuthentication";
        
        private string[] _ignoredRoutes = new string[0];

        public string[] IgnoredRoutes
        {
            get => _ignoredRoutes;

            set
            {
                _ignoredRoutes = value;
                for (var i = 0; i < value.Length; i++)
                {
                    _ignoredRoutes[i] = value[i].ToLower();
                }
            }
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (Array.IndexOf(_ignoredRoutes, actionContext.HttpContext.Request.Path.ToString().ToLower()) >= 0)
            {
                return;
            }

            var configuration = GetConfiguration(actionContext);
            try
            {
                var authToken = GetTokenFromQueryString(actionContext.HttpContext.Request, configuration);
                if (string.IsNullOrEmpty(authToken))
                {
                    authToken = GetTokenFromHeaders(actionContext.HttpContext.Request, configuration);
                }

                var authenticator = new TokenAuthenticator(configuration.Key);
                var authenticationResult = authenticator.Authenticate(authToken);
                if (authenticationResult.IsAuthenticated)
                {
                    return;
                }

                if(!string.IsNullOrEmpty(configuration.CustomRedirect))
                {
                    actionContext.Result = new RedirectResult(configuration.CustomRedirect);
                    return;
                }

                actionContext.Result = new UnauthorizedObjectResult(authenticationResult.Reason);
            }
            catch (Exception)
            {
                actionContext.Result = new BadRequestObjectResult("Your request could not be processed"){ StatusCode = 500 };
            }
        }

        private TokenAuthenticationConfiguration GetConfiguration(ActionExecutingContext actionContext)
        {
            // This comes from here... https://www.devtrends.co.uk/blog/dependency-injection-in-action-filters-in-asp.net-core
            var configuration = actionContext.HttpContext.RequestServices.GetService<IConfiguration>();
            if (configuration == null)
            {
                throw new Exception("Could not load configuration");
            }

            var tokenAuthenticationSection = configuration.GetSection(defaultConfigurationSection);
            var tokenAuthenticationConfiguration = new TokenAuthenticationConfiguration();

            if (tokenAuthenticationSection == null ||!tokenAuthenticationSection.AsEnumerable().Any())
            {
                throw new Exception("Token authentication is not configured");
            }
            
            tokenAuthenticationSection.Bind(tokenAuthenticationConfiguration);    
            return tokenAuthenticationConfiguration;
        }
        
        private static string GetTokenFromQueryString(HttpRequest request, TokenAuthenticationConfiguration configuration)
        {
            if(!string.IsNullOrEmpty(configuration.QueryString))
            {
                return request.Query.FirstOrDefault(a => a.Key == configuration.QueryString)
                    .Value
                    .FirstOrDefault();
            }

            return request.Query.FirstOrDefault(a => a.Key == "api_key")
                    .Value
                    .FirstOrDefault();
        }

        private static string GetTokenFromHeaders(HttpRequest request, TokenAuthenticationConfiguration configuration)
        {
            StringValues authToken;

            if(!string.IsNullOrEmpty(configuration.Header))
            {
                if(request.Headers != null && request.Headers.TryGetValue(configuration.Header, out authToken)){
                    return authToken;
                };
            }        

            if (request.Headers != null && request.Headers.TryGetValue("Authorization", out authToken))
            {
                return authToken.Last().Split(" ").Last();
            }

            return string.Empty;
        }
    }
}
