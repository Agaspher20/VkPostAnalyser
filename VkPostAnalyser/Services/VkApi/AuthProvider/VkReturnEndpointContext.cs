using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;

namespace VkPostAnalyser.Services.VkApi.AuthProvider
{
    public class VkReturnEndpointContext : ReturnEndpointContext
    {
        public VkReturnEndpointContext(IOwinContext context, AuthenticationTicket ticket) : base(context, ticket)
        {
        }
    }
}
