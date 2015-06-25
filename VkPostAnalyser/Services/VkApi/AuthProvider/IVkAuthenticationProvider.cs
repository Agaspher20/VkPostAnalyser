using System.Threading.Tasks;

namespace VkPostAnalyser.Services.VkApi.AuthProvider
{
    public interface IVkAuthenticationProvider
    {
        Task Authenticated(VkAuthenticatedContext context);
        Task ReturnEndpoint(VkReturnEndpointContext context);
    }
}
