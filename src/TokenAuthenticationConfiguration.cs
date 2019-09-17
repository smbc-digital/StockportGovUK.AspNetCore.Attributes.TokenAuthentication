namespace StockportGovUK.AspNetCore.Attributes.TokenAuthentication
{
    public class TokenAuthenticationConfiguration
    {
        public string Key { get; set; }

        public string Header { get; set; }

        public string QueryString { get; set; }
    }
}