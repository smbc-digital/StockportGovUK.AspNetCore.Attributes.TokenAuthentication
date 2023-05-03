namespace StockportGovUK.AspNetCore.Attributes.TokenAuthentication
{
    public class TokenAuthenticationConfiguration
    {
        public string[] Keys { get; set; }
        public string Key { get; set; }
        public string Header { get; set; }
        public string QueryString { get; set; }
        public string CustomRedirect { get; set; }
    }
}