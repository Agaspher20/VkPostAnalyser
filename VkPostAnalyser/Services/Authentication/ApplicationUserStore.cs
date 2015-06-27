using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VkPostAnalyser.Services.Authentication
{
    public class ApplicationUserStore : UserStore<ApplicationUser>, IUserClaimStore<ApplicationUser, string>,
        IUserRoleStore<ApplicationUser, string>,
        IUserPasswordStore<ApplicationUser, string>,
        IUserSecurityStampStore<ApplicationUser, string>,
        IQueryableUserStore<ApplicationUser, string>,
        IUserEmailStore<ApplicationUser, string>,
        IUserPhoneNumberStore<ApplicationUser, string>,
        IUserTwoFactorStore<ApplicationUser, string>,
        IUserLockoutStore<ApplicationUser, string>
    {
        private readonly IAuthenticationManager _authenticationManager;

        public ApplicationUserStore(IAuthenticationManager authenticationManager)
        {
            _authenticationManager = authenticationManager;
        }

        public override async Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            await base.AddLoginAsync(user, login);
        }

        public override async Task<ApplicationUser> FindAsync(UserLoginInfo login)
        {
            //Now userId is vk ALIAS
            var identity = _authenticationManager.GetExternalIdentity(DefaultAuthenticationTypes.ExternalCookie);
            return identity != null ? new ApplicationUser()
            {
                Id = identity.Name,
                UserName = identity.Label
            } : null;
        }

        public override async Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
        {
            return await base.GetLoginsAsync(user);
        }

        public override async Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            await base.RemoveLoginAsync(user, login);
        }

        public override async Task CreateAsync(ApplicationUser user)
        {
            await base.CreateAsync(user);
        }

        public override async Task DeleteAsync(ApplicationUser user)
        {
            await base.DeleteAsync(user);
        }

        public override async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            var identity = _authenticationManager.GetExternalIdentity(DefaultAuthenticationTypes.ExternalCookie);
            return identity != null && identity.Name == userId ? new ApplicationUser()
            {
                Id = identity.Name,
                UserName = identity.Label
            } : null;
        }

        public override async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return await base.FindByNameAsync(userName);
        }

        public override async Task UpdateAsync(ApplicationUser user)
        {
            await base.UpdateAsync(user);
        }

        public override async Task AddClaimAsync(ApplicationUser user, Claim claim)
        {
            await base.AddClaimAsync(user, claim);
        }

        public override async Task<IList<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            return await base.GetClaimsAsync(user);
        }

        public override async Task RemoveClaimAsync(ApplicationUser user, Claim claim)
        {
            await base.RemoveClaimAsync(user, claim);
        }

        public override async Task AddToRoleAsync(ApplicationUser user, string roleName)
        {
            await base.AddToRoleAsync(user, roleName);
        }

        public override async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await base.GetRolesAsync(user);
        }

        public override async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName)
        {
            return await base.IsInRoleAsync(user, roleName);
        }

        public override async Task RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            await base.RemoveFromRoleAsync(user, roleName);
        }

        public override async Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
 	        return await base.GetPasswordHashAsync(user);
        }

        public override async Task<bool> HasPasswordAsync(ApplicationUser user)
        {
 	        return await base.HasPasswordAsync(user);
        }

        public override async Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
 	        await base.SetPasswordHashAsync(user, passwordHash);
        }

        public override async Task<string> GetSecurityStampAsync(ApplicationUser user)
        {
            return await base.GetSecurityStampAsync(user);
        }

        public override async Task SetSecurityStampAsync(ApplicationUser user, string stamp)
        {
            await base.SetSecurityStampAsync(user, stamp);
        }

        public override async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await base.FindByEmailAsync(email);
        }

        public override async Task<string> GetEmailAsync(ApplicationUser user)
        {
            return await base.GetEmailAsync(user);
        }

        public override async Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            return await base.GetEmailConfirmedAsync(user);
        }

        public override async Task SetEmailAsync(ApplicationUser user, string email)
        {
            await base.SetEmailAsync(user, email);
        }

        public override async Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            await base.SetEmailConfirmedAsync(user, confirmed);
        }

        public override async Task<string> GetPhoneNumberAsync(ApplicationUser user)
        {
            return await base.GetPhoneNumberAsync(user);
        }

        public override async Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user)
        {
            return await base.GetPhoneNumberConfirmedAsync(user);
        }

        public override async Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber)
        {
            await base.SetPhoneNumberAsync(user, phoneNumber);
        }

        public override async Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            await base.SetPhoneNumberConfirmedAsync(user, confirmed);
        }

        public override async Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            return await base.GetTwoFactorEnabledAsync(user);
        }

        public override async Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            await base.SetTwoFactorEnabledAsync(user, enabled);
        }

        public override async Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            return await base.GetAccessFailedCountAsync(user);
        }

        public override async Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            return await base.GetLockoutEnabledAsync(user);
        }

        public override async Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            return await base.GetLockoutEndDateAsync(user);
        }

        public override async Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            return await base.IncrementAccessFailedCountAsync(user);
        }

        public override async Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            await base.ResetAccessFailedCountAsync(user);
        }

        public override async Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
        {
            await base.SetLockoutEnabledAsync(user, enabled);
        }

        public override async Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset lockoutEnd)
        {
            await base.SetLockoutEndDateAsync(user, lockoutEnd);
        }
    }
}