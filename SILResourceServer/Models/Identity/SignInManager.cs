using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ResourceServer.Models.Identity
{
    public class SignInManager : SignInManager<User, string>
    {
        public SignInManager(UserManager<User, string> userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager) { }

        public void SignOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        public async Task SignInAsync(
            User user,
            bool isPersistent,
            bool rememberBrowser)
        {
            // Clear any partial cookies from external or two factor partial sign ins
            AuthenticationManager.SignOut(
                DefaultAuthenticationTypes.ApplicationCookie);
            var userIdentity = await user.GenerateUserIdentityAsync(UserManager);
            if (rememberBrowser)
            {
                var rememberBrowserIdentity =
                    AuthenticationManager.CreateTwoFactorRememberBrowserIdentity(user.Id);
                AuthenticationManager.SignIn(
                    new AuthenticationProperties { IsPersistent = isPersistent },
                    userIdentity,
                    rememberBrowserIdentity);
            }
            else
            {
                AuthenticationManager.SignIn(
                    new AuthenticationProperties { IsPersistent = isPersistent },
                    userIdentity);
            }
        }

        private async Task<SignInStatus> SignInOrTwoFactor(
            User user,
            bool isPersistent)
        {
            await SignInAsync(user, isPersistent, false);
            return SignInStatus.Success;
        }

        public async Task<SignInStatus> PasswordSignIn(
            string userName, 
            string password, 
            bool isPersistent, 
            bool shouldLockout)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user == null)
            {
                return SignInStatus.Failure;
            }
  
            if (await UserManager.IsLockedOutAsync(user.Id))
            {
                return SignInStatus.LockedOut;
            }
            if (await UserManager.CheckPasswordAsync(user, password))
            {
                return await SignInOrTwoFactor(user, isPersistent);
            }
  
            if (shouldLockout)
            {
                // If lockout is requested, increment access failed 
                // count which might lock out the user
                await UserManager.AccessFailedAsync(user.Id);
                if (await UserManager.IsLockedOutAsync(user.Id))
                {
                    return SignInStatus.LockedOut;
                }
            }
            return SignInStatus.Failure;
        }

        public async Task<SignInStatus> PasswordSignInSharep(
            string userName)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user == null)
            {
                return SignInStatus.Failure;
            }
            ResourceServer.Models.Identity.UserManager GestorUsuario = UserManager as ResourceServer.Models.Identity.UserManager;
            if (await GestorUsuario.CheckSharepointLoginAsync(user))
            {
                return await SignInOrTwoFactor(user, false);
            }
            return SignInStatus.Failure;
        } 
    }
}