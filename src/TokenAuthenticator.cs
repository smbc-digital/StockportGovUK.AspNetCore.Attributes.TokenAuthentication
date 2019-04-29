namespace StockportGovUK.AspNetCore.Attributes.TokenAuthentication
{
    public class TokenAuthenticator
    {
        private readonly string _environmentAuthToken;

        public TokenAuthenticator(string environmentAuthToken)
        {
            _environmentAuthToken = environmentAuthToken;
        }

        public AuthenticationResult Authenticate(string headerAuthToken)
        {
            var isAuthenticated = false;
            string reason;

            if (string.IsNullOrWhiteSpace(_environmentAuthToken))
            {
                reason = AuthenticationConstants.NoAuthTokenEnvironmentVariableError;
            }
            else if (string.IsNullOrWhiteSpace(headerAuthToken) || !this.IsTokenValid(headerAuthToken))
            {
                reason = AuthenticationConstants.InvalidOrMissingAuthToken;

                if (!string.IsNullOrWhiteSpace(headerAuthToken))
                {
                    reason = string.Format("{0}: {1}", reason, headerAuthToken);
                }
            }
            else
            {
                reason = AuthenticationConstants.ValidAuthTokenReceived;
                isAuthenticated = true;
            }

            return new AuthenticationResult(isAuthenticated, reason);
        }

        private bool IsTokenValid(string tokenToCheck)
        {
            return _environmentAuthToken.Equals(tokenToCheck);
        }
    }
}