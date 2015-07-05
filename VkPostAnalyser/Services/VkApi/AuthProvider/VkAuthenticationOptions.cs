using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Net.Http;
using VkPostAnalyser.Domain.Services.VkApi;
using VKSharp.Data.Api;

namespace VkPostAnalyser.Services.VkApi.AuthProvider
{
    public class VkAuthenticationOptions : AuthenticationOptions
    {
        public VkAuthenticationOptions()
            : base(VkConstants.VkAuthenticationType)
        {
            Caption = VkConstants.VkAuthenticationType;
            CallbackPath = new PathString(VkConstants.SignInPath);
            AuthenticationMode = AuthenticationMode.Passive;
            Version = VkConstants.ApiVersion;
            BackchannelTimeout = TimeSpan.FromSeconds(60);
        }
        
        public int ClientId { get; set; }
        public string ClientSecret { get; set; }
        public ICertificateValidator BackchannelCertificateValidator { get; set; }
        public TimeSpan BackchannelTimeout { get; set; }
        public HttpMessageHandler BackchannelHttpHandler { get; set; }
        
        public string Caption
        {
            get { return Description.Caption; }
            set { Description.Caption = value; }
        }
        
        public PathString CallbackPath { get; set; }
        public string SignInAsAuthenticationType { get; set; }
        public IVkAuthenticationProvider Provider { get; set; }
        public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }
        public string StoreState { get; set; }
        public VKPermission Permissions { get; set; }
        public string Version { get; set; }
    }
}
