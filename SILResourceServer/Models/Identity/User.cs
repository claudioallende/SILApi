using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ResourceServer.Models.Identity
{
    public class User : IUser<string>
    {
        private string _userName;
        private string _password;

        public virtual string Id { get; set; }
        public virtual string UserName
        {
            get { return this._userName.Trim(); }
            set { this._userName = value; }
        }
        public virtual string Password 
        {
            get { return this._password.Trim(); }
            set { this._password = value; }
        }

        public virtual async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<User, string> manager)
        {
            // Note the authenticationType must match the one defined in
            // CookieAuthenticationOptions.AuthenticationType 
            var userIdentity = await manager.CreateIdentityAsync(
                this, DefaultAuthenticationTypes.ApplicationCookie);
            userIdentity.AddClaims(await manager.GetClaimsAsync(this.UserName));
            return userIdentity;
        }
    }
}