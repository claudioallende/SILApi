using Microsoft.AspNet.Identity;
using NHibernate;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace ResourceServer.Models.Identity
{
    public class UserStore : IUserStore<User, string>,
        IUserPasswordStore<User, string>,
        IUserLockoutStore<User, string>,
        IUserTwoFactorStore<User, string>
    {
        private readonly ISession session;

        public UserStore() {}

        public UserStore(ISession session)
        {
            this.session = session;
        }

        #region IUserStore<User, int>
        public Task CreateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByIdAsync(string userId)
        {
            return Task.Run(() => session.Get<User>(userId));
        }

        public Task<User> FindByNameAsync(string userName)
        {
            User user = session.Query<User>().FirstOrDefault(u => u.UserName.Trim().ToUpper() == userName.Trim().ToUpper());
            return Task.FromResult(user);
        }

        public Task UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IUserPasswordStore<User, int>
        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(user.Password);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(true);
        }
        #endregion

        #region IUserLockoutStore<User, int>
        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            return Task.FromResult(DateTimeOffset.MaxValue);
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            return Task.CompletedTask;
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            return Task.FromResult(0);
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            return Task.CompletedTask;
        }

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task.FromResult(false);
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            return Task.CompletedTask;
        }
        #endregion

        #region IUserTwoFactorStore<User, int>
        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            return Task.CompletedTask;
        }

        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return Task.FromResult(false);
        }
        #endregion

        public void Dispose()
        {
            //do nothing
        }
    }
}