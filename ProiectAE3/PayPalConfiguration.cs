using PayPal.Api;

namespace ProiectAE3
{
    public static class PayPalConfiguration
    {
        static PayPalConfiguration()
        {
        }

        public static Dictionary<string, string> GetConfig(string mode)
        {
            return new Dictionary<string, string>()
            {
                {"mode", mode }
            };
        }

        public static APIContext GetAPIContext(string clientId, string clientSecret, string mode)
        {
            var apiContext = new APIContext(GetAccessToken(clientId, clientSecret, mode));
            apiContext.Config = GetConfig(mode);
            return apiContext;
        }

        private static string GetAccessToken(string clientId, string clientSecret, string mode)
        {
            try
            {
                var accessToken = new OAuthTokenCredential(clientId, clientSecret, new Dictionary<string, string>()
            {
                {"mode", mode }
            }).GetAccessToken();

                return accessToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obtaining access token: {ex.Message}");
                throw;
            }
        }
    }
}
