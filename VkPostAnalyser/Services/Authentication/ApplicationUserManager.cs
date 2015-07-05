using Microsoft.AspNet.Identity;
using VkPostAnalyser.Domain.Model;

namespace VkPostAnalyser.Services.Authentication
{
    public class ApplicationUserManager : UserManager<ApplicationUser, int>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, int> store) : base(store) {}
    }
}
