using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace StockportGovUK.AspNetCore.Attributes.TokenAuthentication.Tests
{
    public class TokenAuthenticatorAttributeTests
    {
        private Mock<RouteData> mockRouteData = new Mock<RouteData>();
        private Mock<ActionDescriptor> mockActionDescriptor = new Mock<ActionDescriptor>();
        private Mock<Controller> mockController = new Mock<Controller>();

        private Mock<HttpContext> BuildMockHttpContext(string config = "appsettings.json") {
            var configuration = ConfigurationLoadHelper.GetConfiguration(config);

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(provider => provider.GetService(typeof(IConfiguration)))
                .Returns(configuration);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext
                .SetupGet(context => context.RequestServices)
                .Returns(mockServiceProvider.Object);

            return mockHttpContext;
        }

        [Fact]
        public void TokenAuthenticatorAttribute_Returns_UnauthorizedAuthenticationResult_WhenIncorrect_ApiKeyIsInQueryString()
        {
            var queryDictionary = new Dictionary<string, StringValues>();
            queryDictionary.Add("api_key", new StringValues("abc1234"));
            
            var queryCollection = new QueryCollection(queryDictionary);
            
            var mockHttpContext = BuildMockHttpContext();;
            mockHttpContext.Setup(_ => _.Request.Query).Returns(queryCollection);
            

            var actionContext = new ActionContext(mockHttpContext.Object, mockRouteData.Object, mockActionDescriptor.Object);
            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object>(), mockController.Object);
            var tokenAuthenticationAttribute = new TokenAuthenticationAttribute();
            
            // Act 
            tokenAuthenticationAttribute.OnActionExecuting(actionExecutingContext);
            
            // Assert
            Assert.IsType<UnauthorizedObjectResult>(actionExecutingContext.Result);
        }

        [Fact]
        public void TokenAuthenticatorAttribute_Returns_RedirectResult_WhenIncorrect_ApiKeyIsInQueryString_AndCustomRedirectSpecified()
        {
            var queryDictionary = new Dictionary<string, StringValues>();
            queryDictionary.Add("api_key", new StringValues("abc1234"));
            
            var queryCollection = new QueryCollection(queryDictionary);
            
            var mockHttpContext = BuildMockHttpContext("appsettingsCustomRedirect.json");
            mockHttpContext.Setup(_ => _.Request.Query).Returns(queryCollection);    

            var actionContext = new ActionContext(mockHttpContext.Object, mockRouteData.Object, mockActionDescriptor.Object);
            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object>(), mockController.Object);
            var tokenAuthenticationAttribute = new TokenAuthenticationAttribute();
            
            // Act 
            tokenAuthenticationAttribute.OnActionExecuting(actionExecutingContext);
            
            // Assert
            Assert.IsType<RedirectResult>(actionExecutingContext.Result);
        }
        
        [Fact]
        public void TokenAuthenticatorAttribute_Returns_UnauthorizedAuthenticationResult_WhenIncorrect_ApiKeyIsInQueryString_AndUsingAlternativeQueryString()
        {
            var queryDictionary = new Dictionary<string, StringValues>();
            queryDictionary.Add("MyAlternativeQueryString", new StringValues("abc1234"));
            
            var queryCollection = new QueryCollection(queryDictionary);
            
            var mockHttpContext = BuildMockHttpContext("appsettingsQueryString.json");
            mockHttpContext.Setup(_ => _.Request.Query).Returns(queryCollection);
            

            var actionContext = new ActionContext(mockHttpContext.Object, mockRouteData.Object, mockActionDescriptor.Object);
            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object>(), mockController.Object);
            var tokenAuthenticationAttribute = new TokenAuthenticationAttribute();
            
            // Act 
            tokenAuthenticationAttribute.OnActionExecuting(actionExecutingContext);
            
            // Assert
            Assert.IsType<UnauthorizedObjectResult>(actionExecutingContext.Result);
        }
        
        [Fact]
        public void TokenAuthenticatorAttribute_Returns_AuthorizedAuthenticationResult_WhenCorrect_ApiKeyIsInQueryString()
        {
            var configuration = ConfigurationLoadHelper.GetConfiguration();
            var mockHttpContext = BuildMockHttpContext();;
            var queryDictionary = new Dictionary<string, StringValues>();
            queryDictionary.Add("api_key", new StringValues("abc12345"));
            var queryCollection = new QueryCollection(queryDictionary);
        
            mockHttpContext
                .Setup(_ => _.Request.Query)
                .Returns(queryCollection);
            
            var actionContext = new ActionContext(mockHttpContext.Object, mockRouteData.Object, mockActionDescriptor.Object);
            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object>(), mockController.Object);
            var tokenAuthenticationAttribute = new TokenAuthenticationAttribute();
            
            // Act 
            tokenAuthenticationAttribute.OnActionExecuting(actionExecutingContext);
            
            // Assert
            Assert.IsNotType<UnauthorizedObjectResult>(actionExecutingContext.Result);
            Assert.IsNotType<BadRequestObjectResult>(actionExecutingContext.Result);
        }

        [Fact]
        public void TokenAuthenticatorAttribute_Returns_UnauthorizedAuthenticationResult_WhenApiKeyIsInHeader()
        {          
            var dictionary = new Dictionary<string, StringValues>();
            dictionary.Add("Authorization", new StringValues("BEARER abc1234"));
                        
            var mockHttpContext = BuildMockHttpContext();;
            mockHttpContext.Setup(_ => _.Request.Headers).Returns(new HeaderDictionary(dictionary));
            mockHttpContext.Setup(_ => _.Request.Query).Returns(new QueryCollection());

            var actionContext = new ActionContext(mockHttpContext.Object, mockRouteData.Object, mockActionDescriptor.Object);
            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object>(), mockController.Object);
            var tokenAuthenticationAttribute = new TokenAuthenticationAttribute();
            
            // Act 
            tokenAuthenticationAttribute.OnActionExecuting(actionExecutingContext);
            
            // Assert
            Assert.IsType<UnauthorizedObjectResult>(actionExecutingContext.Result);
        }

                [Fact]
        public void TokenAuthenticatorAttribute_Returns_UnauthorizedAuthenticationResult_WhenApiKeyIsInHeader_AndAlternativeHeaderIsUser()
        {          
            var dictionary = new Dictionary<string, StringValues>();
            dictionary.Add("MyAlternativeHeader", new StringValues("abc1234"));
                        
            var mockHttpContext = BuildMockHttpContext("appsettingsHeader.json");
            mockHttpContext.Setup(_ => _.Request.Headers).Returns(new HeaderDictionary(dictionary));
            mockHttpContext.Setup(_ => _.Request.Query).Returns(new QueryCollection());

            var actionContext = new ActionContext(mockHttpContext.Object, mockRouteData.Object, mockActionDescriptor.Object);
            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object>(), mockController.Object);
            var tokenAuthenticationAttribute = new TokenAuthenticationAttribute();
            
            // Act 
            tokenAuthenticationAttribute.OnActionExecuting(actionExecutingContext);
            
            // Assert
            Assert.IsType<UnauthorizedObjectResult>(actionExecutingContext.Result);
        }

        [Fact]
        public void TokenAuthenticatorAttribute_Returns_AuthorizedAuthenticationResult_WhenCorrectApiKeyIsInHeader()
        {          
            var dictionary = new Dictionary<string, StringValues>();
            dictionary.Add("Authorization", new StringValues("BEARER abc12345"));
                        
            var mockHttpContext = BuildMockHttpContext();
            mockHttpContext.Setup(_ => _.Request.Headers).Returns(new HeaderDictionary(dictionary));
            mockHttpContext.Setup(_ => _.Request.Query).Returns(new QueryCollection());

            var actionContext = new ActionContext(mockHttpContext.Object, mockRouteData.Object, mockActionDescriptor.Object);
            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object>(), mockController.Object);
            var tokenAuthenticationAttribute = new TokenAuthenticationAttribute();
            
            // Act 
            tokenAuthenticationAttribute.OnActionExecuting(actionExecutingContext);
            
            // Assert
            Assert.IsNotType<UnauthorizedObjectResult>(actionExecutingContext.Result);
            Assert.IsNotType<BadRequestObjectResult>(actionExecutingContext.Result);
        }

        [Fact]
        public void TokenAuthenticatorAttribute_Returns_BadRequestObjectResult_WhenExceptionIsThrown()
        {          
            var mockHttpContext = BuildMockHttpContext();
            mockHttpContext.Setup(_ => _.Request.Query).Throws(new Exception("This is a test exception"));

            var actionContext = new ActionContext(mockHttpContext.Object, mockRouteData.Object, mockActionDescriptor.Object);
            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object>(), mockController.Object);
            var tokenAuthenticationAttribute = new TokenAuthenticationAttribute();
            
            // Act 
            tokenAuthenticationAttribute.OnActionExecuting(actionExecutingContext);
            
            // Assert
            Assert.IsType<BadRequestObjectResult>(actionExecutingContext.Result);
        }
    }
}
