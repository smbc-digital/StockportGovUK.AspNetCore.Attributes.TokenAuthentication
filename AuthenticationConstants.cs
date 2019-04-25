namespace StockportGovUK.AspNetCore.Attributes.TokenAuthentication
{
    public static class AuthenticationConstants
    {
        public const string NoAuthTokenEnvironmentVariableError = "No authentication token set up on this environment";
        public const string InvalidOrMissingAuthToken = "Invalid or missing authentication token";
        public const string ValidAuthTokenReceived = "Valid authentication token received";
        // public const string SomethingTerribleHasHappenedError = "Something terrible has happened";
        // public const string AnUnexpectedErrorOccurred = "An unexpected error occurred";
    }
}