using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.General
{
    public class ApplicationRole : IdentityRole
    {
        public IEnumerable<ApplicationUserRole> UserRoles { get; set; }

        public ApplicationRole()
        {
        }
        
        public ApplicationRole(string roleName)
            : base(roleName)
        {
        }
    }
}
