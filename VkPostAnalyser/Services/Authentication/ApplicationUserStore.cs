using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkPostAnalyser.Domain.Model;
using VkPostAnalyser.Domain.Services.VkApi;

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
            return await RetrieveUserFromExternalClaimIdentity(new UserLoginInfo(
                loginProvider: DefaultAuthenticationTypes.ExternalCookie,
                providerKey: login.ProviderKey
            ));
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
        {
            return Task.Run<IList<UserLoginInfo>>(() => new List<UserLoginInfo>(0));
        }

        public Task CreateAsync(ApplicationUser user)
        {
            return Task.Run(() => { });
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            return Task.Run(() => { });
        }

        public async Task<ApplicationUser> FindByIdAsync(int userId)
        {
            return await RetrieveUserFromExternalClaimIdentity(new UserLoginInfo(
                loginProvider: DefaultAuthenticationTypes.ApplicationCookie,
                providerKey: userId.ToString()
            )) ?? await RetrieveUserFromExternalClaimIdentity(new UserLoginInfo(
                loginProvider: DefaultAuthenticationTypes.ExternalCookie,
                providerKey: userId.ToString()
            ));
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return null;
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            return Task.Run(() => { });
        }

        public void Dispose() { }

        public Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            return Task.Run(() => { });
        }

        public Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            return Task.Run(() => { });
        }

        public Task AddToRoleAsync(ApplicationUser user, string roleName)
        {
            return Task.Run(() => { });
        }

        public Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return Task.Run<IList<string>>(() => new List<string>());
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName) { return Task.Run(() => false); }

        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            return Task.Run(() => { });
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            return Task.Run(() => string.Empty);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return Task.Run(() => false);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            return Task.Run(() => { });
        }

        public Task<string> GetSecurityStampAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                var stamp = Guid.NewGuid().ToByteArray();
                return Convert.ToBase64String(stamp, 0, stamp.Length);
            });
        }

        public Task SetSecurityStampAsync(ApplicationUser user, string stamp)
        {
            return Task.Run(() => { });
        }

        public IQueryable<ApplicationUser> Users { get { return null; } }

        public Task<ApplicationUser> FindByEmailAsync(string email) { return null; }

        public Task<string> GetEmailAsync(ApplicationUser user) { return null; }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            return Task.Run(() => false);
        }

        public Task SetEmailAsync(ApplicationUser user, string email)
        {
            return Task.Run(() => { });
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            return Task.Run(() => { });
        }

        public Task<string> GetPhoneNumberAsync(ApplicationUser user)
        {
            return Task.Run(() => string.Empty);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user)
        {
            return Task.Run(() => false);
        }

        public Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber)
        {
            return Task.Run(() => { });
        }

        public Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            return Task.Run(() => { });
        }

        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            return Task.Run(() => false);
        }

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            return Task.Run(() => { });
        }

        public Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            return Task.Run(() => 0);
        }

        public Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            return Task.Run(() => false);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            return Task.Run(() => DateTimeOffset.MinValue);
        }

        public Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            return Task.Run(() => 0);
        }

        public Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            return Task.Run(() => { });
        }

        public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
        {
            return Task.Run(() => { });
        }

        public Task SetLockoutEndDateAsync(ApplicationUser user, System.DateTimeOffset lockoutEnd)
        {
            return Task.Run(() => { });
        }

        private async Task<ApplicationUser> RetrieveUserFromExternalClaimIdentity(UserLoginInfo login)
        {
            var identity = await _authenticationManager.GetExternalIdentityAsync(login.LoginProvider);
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
