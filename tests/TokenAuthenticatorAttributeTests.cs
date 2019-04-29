using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StockportGovUK.AspNetCore.Attributes.TokenAuthentication;
using Xunit;

namespace StockportGovUK.AspNetCore.Attributes.TokenAuthentication.Tests
{
    public class TokenAuthenticatorAttributeTests
    {
        private Mock<IServiceProvider> MockServiceProvider { get {
            var configuration = ConfigurationLoadHelper.GetConfiguration();
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(provider => provider.GetService(typeof(IConfiguration)))
                .Returns(configuration);

            return mockServiceProvider;
        }}

        private Mock<HttpContext> MockHttpContext { get {
            var configuration = ConfigurationLoadHelper.GetConfiguration();

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(provider => provider.GetService(typeof(IConfiguration)))
                .Returns(configuration);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext
                .SetupGet(context => context.RequestServices)
                .Returns(mockServiceProvider.Object);

            return mockHttpContext;
        }}

        // [Fact]
        // public void TokenAuthenticatorAttribute_Returns_UnauthjorizedAuthenticationResult_WhenIncorrect_ApiKeyIsInQueryString()
        // {
        //     var queryDictionary = new Dictionary<string, StringValues>();
        //     queryDictionary.Add("api_key", new StringValues("abc1234"));
            
        //     var queryCollection = new QueryCollection(queryDictionary);
            
        //     var mockHttpContext = MockHttpContext;
        //     mockHttpContext.Setup(_ => _.Request.Query).Returns(queryCollection);
            
        //     var mockRouteData = new Mock<RouteData>();
        //     var mockActionDescriptor = new Mock<ActionDescriptor>();
        //     var mockController = new Mock<Controller>();
        //     var actionContext = new ActionContext(mockHttpContext.Object, mockRouteData.Object, mockActionDescriptor.Object);
        //     var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object>(), mockController.Object);
        //     var tokenAuthenticationAttribute = new TokenAuthenticationAttribute();
            
        //     // Act 
        //     tokenAuthenticationAttribute.OnActionExecuting(actionExecutingContext);
            
        //     // Assert
        //     Assert.IsType<UnauthorizedObjectResult>(actionExecutingContext.Result);
        // }
        
        [Fact]
        public void TokenAuthenticatorAttribute_Returns_AuthorizedAuthenticationResult_WhenIncorrect_ApiKeyIsInQueryString()
        {
            var configuration = ConfigurationLoadHelper.GetConfiguration();
            
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(provider => provider.GetService(typeof(IConfiguration)))
                .Returns(configuration);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext
                .SetupGet(context => context.RequestServices)
                .Returns(mockServiceProvider.Object);

            var queryDictionary = new Dictionary<string, StringValues>();
            queryDictionary.Add("api_key", new StringValues("abc12345"));
            var queryCollection = new QueryCollection(queryDictionary);
        
            mockHttpContext
                .Setup(_ => _.Request.Query)
                .Returns(queryCollection);
            
            var mockRouteData = new Mock<RouteData>();
            var mockActionDescriptor = new Mock<ActionDescriptor>();
            var mockController = new Mock<Controller>();
            var actionContext = new ActionContext(mockHttpContext.Object, mockRouteData.Object, mockActionDescriptor.Object);
            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object>(), mockController.Object);
            var tokenAuthenticationAttribute = new TokenAuthenticationAttribute();
            
            // Act 
            tokenAuthenticationAttribute.OnActionExecuting(actionExecutingContext);
            
            // Assert
            Assert.IsNotType<UnauthorizedObjectResult>(actionExecutingContext.Result);
            Assert.IsNotType<BadRequestObjectResult>(actionExecutingContext.Result);
        }

        // [Fact]
        // public void TokenAuthenticatorAttribute_Returns_UnauthorizedAuthenticationResult_WhenApiKeyIsInHeader()
        // {          
        //     var dictionary = new Dictionary<string, StringValues>();
        //     dictionary.Add("Authorization", new StringValues("BEARER abc1234"));
                        
        //     var mockHttpContext = MockHttpContext;
        //     mockHttpContext.Setup(_ => _.Request.Headers).Returns(new HeaderDictionary(dictionary));
        //     mockHttpContext.Setup(_ => _.Request.Query).Returns(new QueryCollection());

        //     var mockRouteData = new Mock<RouteData>();
        //     var mockActionDescriptor = new Mock<ActionDescriptor>();
        //     var mockController = new Mock<Controller>();
        //     var actionContext = new ActionContext(mockHttpContext.Object, mockRouteData.Object, mockActionDescriptor.Object);
        //     var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object>(), mockController.Object);
        //     var tokenAuthenticationAttribute = new TokenAuthenticationAttribute();
            
        //     // Act 
        //     tokenAuthenticationAttribute.OnActionExecuting(actionExecutingContext);
            
        //     // Assert
        //     Assert.IsType<UnauthorizedObjectResult>(actionExecutingContext.Result);
        // }
    }
}
