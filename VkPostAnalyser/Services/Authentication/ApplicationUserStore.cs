using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VkPostAnalyser.Services.VkApi.AuthProvider;

namespace VkPostAnalyser.Services.Authentication
{
    public class ApplicationUserStore : IUserStore<ApplicationUser, int>, IUserLoginStore<ApplicationUser, int>,
        IUserRoleStore<ApplicationUser, int>,
        IUserPasswordStore<ApplicationUser, int>,
        IUserSecurityStampStore<ApplicationUser, int>,
        IQueryableUserStore<ApplicationUser, int>,
        IUserEmailStore<ApplicationUser, int>,
        IUserPhoneNumberStore<ApplicationUser, int>,
        IUserTwoFactorStore<ApplicationUser, int>,
        IUserLockoutStore<ApplicationUser, int>
    {
        private readonly IAuthenticationManager _authenticationManager;

        public ApplicationUserStore(IAuthenticationManager authenticationManager)
        {
            _authenticationManager = authenticationManager;
        }

        public async Task<ApplicationUser> FindAsync(UserLoginInfo login)
        {
            return await RetrieveUserFromExternalClaimIdentity(login);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
        {
            return new List<UserLoginInfo>(0);
        }

        public async Task CreateAsync(ApplicationUser user)
        {
        }

        public async Task DeleteAsync(ApplicationUser user)
        {
        }

        public async Task<ApplicationUser> FindByIdAsync(int userId)
        {
            return await RetrieveUserFromExternalClaimIdentity(null);
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return null;
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
        }

        public void Dispose() { }

        public async Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
        {   
        }

        public async Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
        }

        public async Task AddToRoleAsync(ApplicationUser user, string roleName) { }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user) { return new List<string>(); }

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName) { return false; }

        public async Task RemoveFromRoleAsync(ApplicationUser user, string roleName) { }

        public async Task<string> GetPasswordHashAsync(ApplicationUser user) { return string.Empty; }

        public async Task<bool> HasPasswordAsync(ApplicationUser user) { return false; }

        public async Task SetPasswordHashAsync(ApplicationUser user, string passwordHash) { }

        public async Task<string> GetSecurityStampAsync(ApplicationUser user)
        {
            var stamp = Guid.NewGuid().ToByteArray();
            return Convert.ToBase64String(stamp, 0, stamp.Length);
        }

        public async Task SetSecurityStampAsync(ApplicationUser user, string stamp) { }

        public IQueryable<ApplicationUser> Users { get { return null; } }

        public async Task<ApplicationUser> FindByEmailAsync(string email) { return null; }

        public async Task<string> GetEmailAsync(ApplicationUser user) { return null; }

        public async Task<bool> GetEmailConfirmedAsync(ApplicationUser user) { return false; }

        public async Task SetEmailAsync(ApplicationUser user, string email) { }

        public async Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed) { }

        public async Task<string> GetPhoneNumberAsync(ApplicationUser user) { return null; }

        public async Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user) { return false; }

        public async Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber) { }

        public async Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed) { }

        public async Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user) { return false; }

        public async Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled) { }

        public async Task<int> GetAccessFailedCountAsync(ApplicationUser user) { return 0; }

        public async Task<bool> GetLockoutEnabledAsync(ApplicationUser user) { return false; }

        public async Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user) { return DateTimeOffset.MinValue; }

        public async Task<int> IncrementAccessFailedCountAsync(ApplicationUser user) { return 0; }

        public async Task ResetAccessFailedCountAsync(ApplicationUser user) { }

        public async Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled) { }

        public async Task SetLockoutEndDateAsync(ApplicationUser user, System.DateTimeOffset lockoutEnd) { }

        private async Task<ApplicationUser> RetrieveUserFromExternalClaimIdentity(UserLoginInfo login)
        {
            var identity = await _authenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
            if (identity != null)
            {
                var applicationUser = new ApplicationUser { Id = -1 };
                foreach (var claim in identity.Claims)
                {
                    switch (claim.Type)
                    {
                        case VkConstants.ClaimTypes.Token:
                            applicationUser.Token = claim.Value;
                            break;
                        case VkConstants.ClaimTypes.Id:
                            applicationUser.Id = int.Parse(claim.Value);
                            break;
                        case VkConstants.ClaimTypes.Alias:
                            applicationUser.UserAlias = claim.Value;
                            break;
                        case VkConstants.ClaimTypes.FullName:
                            applicationUser.UserName = claim.Value;
                            break;
                        default: break;
                    }
                }
                return applicationUser;
            }
            return null;
        }
    }
}
