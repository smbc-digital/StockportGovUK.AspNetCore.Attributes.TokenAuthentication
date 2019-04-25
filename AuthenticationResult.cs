namespace StockportGovUK.AspNetCore.Attributes.TokenAuthentication
{
    public class AuthenticationResult
    {
        public readonly bool IsAuthenticated;
        public readonly string Reason;

        public AuthenticationResult(bool isAuthenticated, string reason)
        {
            IsAuthenticated = isAuthenticated;
            Reason = reason;
        }
    }
}