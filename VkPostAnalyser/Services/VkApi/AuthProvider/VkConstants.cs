using System.Security.Claims;

namespace VkPostAnalyser.Services.VkApi.AuthProvider
{
    internal static class VkConstants
    {
        public static class ClaimTypes
        {
            public const string Token = "urn:vkontakte:accesstoken";
            public const string FullName = "urn:vkontakte:name";
            public const string AvatarLink = "urn:vkontakte:link";
            public const string Alias = ClaimsIdentity.DefaultNameClaimType;
            public const string Id = System.Security.Claims.ClaimTypes.NameIdentifier;
        }

        public const string VkAuthenticationType = "Vkontakte";
        public const string VkBaseUrl = "http://vk.com";
        public const string TokenEndpoint = "https://oauth.vk.com/access_token";
        public const string GraphApiEndpoint = "https://api.vk.com/method/";
    }
}
