using System;
using System.Security.Claims;
using System.Security.Principal;

namespace OmegaGraf.Compose
{
    public static class KeyDatabase
    {
        private static string activeApiKey;

        public static ClaimsPrincipal GetClaimFromApiKey(string apiKey)
        {
            if (activeApiKey != apiKey)
            {
                return null;
            }

            return new ClaimsPrincipal(new GenericIdentity(apiKey, "stateless"));
        }

        public static bool ValidateKey(string apiKey)
        {
            if (activeApiKey == apiKey)
            {
                return true;
            }

            return false;
        }

        public static string CreateKey()
        {
            var key = Guid.NewGuid().ToString().Replace("-", null);
            activeApiKey = key;

            return key;
        }

        public static string CreateKey(string key)
        {
            activeApiKey = key;

            return key;
        }
    }
}