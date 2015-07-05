using System.Security.Claims;

namespace VkPostAnalyser.Domain.Services.VkApi
{
    public static class VkConstants
    {
        public static class ClaimTypes
        {
            public const string Token = "urn:vkontakte:accesstoken";
            public const string FullName = "urn:vkontakte:name";
            public const string AvatarLink = "urn:vkontakte:link";
            public const string Alias = ClaimsIdentity.DefaultNameClaimType;
            public const string Id = System.Security.Claims.ClaimTypes.NameIdentifier;
        }

        public const string ApiVersion = "5.3";
        public const string VkAuthenticationType = "Vkontakte";
        public const string VkBaseUrl = "http://vk.com";
        public const string TokenEndpoint = "https://oauth.vk.com/access_token";
        public const string GraphApiEndpoint = "https://api.vk.com/method/";
        public const string SignInPath = "/signin-vkontakte";
        public const int MaxWallPageSize = 100; // Max count of posts which wall.get returns
        public const int MaxRequestsCountPerSecond = 3;
    }
}
