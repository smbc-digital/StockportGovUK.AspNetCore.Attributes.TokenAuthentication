using System;
using Xunit;
using StockportGovUK.AspNetCore.Attributes.TokenAuthentication;

namespace StockportGovUK.AspNetCore.Attributes.TokenAuthentication.Tests
{
    public class TokenAuthenticatorTests
    {
        [Fact]
        public void TokenAuthenticator_Returns_ValidAuthenticationResult()
        {
            // Arrange 
            var tokenAuthenticator = new TokenAuthenticator("abc12345");
            
            // Act 
            var result = tokenAuthenticator.Authenticate("abc12345");
            
            // Assert
            Assert.True(result.IsAuthenticated);
            Assert.Equal(result.Reason, AuthenticationConstants.ValidAuthTokenReceived);
        }

        [Fact]
        public void TokenAuthenticator_WithEmptyAuthToken_Returns_ValidAuthenticationResult()
        {
            // Arrange 
            var tokenAuthenticator = new TokenAuthenticator(string.Empty);
            
            // Act 
            var result = tokenAuthenticator.Authenticate("abc12345");
            
            // Assert
            Assert.False(result.IsAuthenticated);
            Assert.Equal(result.Reason, AuthenticationConstants.NoAuthTokenEnvironmentVariableError);
        }

        [Fact]
        public void TokenAuthenticator_WithEmptEnvironmentAuthToken_Returns_NotValidAuthenticationResult()
        {
            // Arrange 
            var tokenAuthenticator = new TokenAuthenticator("abc12345");
            
            // Act 
            var result = tokenAuthenticator.Authenticate(string.Empty);
            
            // Assert
            Assert.False(result.IsAuthenticated);
            Assert.Equal(result.Reason, AuthenticationConstants.InvalidOrMissingAuthToken);
        }

        [Fact]
        public void TokenAuthenticator_WithEmptyHeaderAuthToken_Returns_NotValidAuthenticationResult()
        {
            // Arrange 
            var tokenAuthenticator = new TokenAuthenticator("abc12345");
            
            // Act 
            var result = tokenAuthenticator.Authenticate("IncorrectToken");
            var expectedResult = $"{AuthenticationConstants.InvalidOrMissingAuthToken}: IncorrectToken";
            // Assert
            Assert.False(result.IsAuthenticated);
            Assert.Equal(result.Reason, expectedResult);
        }

    }
}
