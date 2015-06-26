//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.Owin.Security;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace VkPostAnalyser.Services.Authentication
//{
//    public class ApplicationUserStore : IUserStore<ApplicationUser>, IUserLoginStore<ApplicationUser>
//    {
//        private readonly IRepository _repository;
//        private readonly IAuthenticationManager _authenticationManager;

//        public ApplicationUserStore(IRepository repository, IAuthenticationManager authenticationManager)
//        {
//            _repository = repository;
//            _authenticationManager = authenticationManager;
//        }

//        public void Dispose()
//        {
//        }

//        public async Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
//        {
//            await CreateAsync(user);
//        }

//        public async Task<ApplicationUser> FindAsync(UserLoginInfo login)
//        {
//            var claim = await _authenticationManager.GetExternalIdentityAsync(login.LoginProvider);
//            return await FindByIdAsync(claim.Name);
//        }

//        public async Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
//        {
//            return await Task.Run(() => new List<UserLoginInfo>
//            {
//                //new UserLoginInfo
//                //{
//                //    LoginProvider = ApplicationU
//                //}
//            });
//        }

//        public async Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login)
//        {
//            await Task.Run(() => { });
//        }

//        public async Task CreateAsync(ApplicationUser user)
//        {
//            if (user == null)
//            {
//                throw new ArgumentNullException("user");
//            }
//            await _repository.SaveUserAsync(user);
//        }

//        public async Task DeleteAsync(ApplicationUser user)
//        {
//            await _repository.DeleteUserAsync(user);
//        }

//        public async Task<ApplicationUser> FindByIdAsync(string userId)
//        {
//            return await Task.Run<ApplicationUser>(() => _repository.Users.FirstOrDefault(u => u.Id == userId));
//        }

//        public async Task<ApplicationUser> FindByNameAsync(string userName)
//        {
//            return await Task.Run<ApplicationUser>(() => _repository.Users.FirstOrDefault(u => u.UserName == userName));
//        }

//        public async Task UpdateAsync(ApplicationUser user)
//        {
//            await _repository.SaveChangesAsync();
//        }
//    }
//}