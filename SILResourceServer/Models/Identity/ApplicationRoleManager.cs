using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Identity
{
    public class ApplicationRoleManager : RoleManager<Role, int>
    {
        public ApplicationRoleManager(IRoleStore<Role, int> roleStore)
            : base(roleStore)
        {
        }

        //public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        //{
        //    return new ApplicationRoleManager(new RoleDatabaseContext().Roles);
        //}
    }
}