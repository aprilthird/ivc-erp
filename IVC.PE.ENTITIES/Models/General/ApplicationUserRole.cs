using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using static IVC.PE.CORE.Helpers.ConstantHelpers;

namespace IVC.PE.ENTITIES.Models.General
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; }
        
        public virtual ApplicationRole Role { get; set; }

        public int PermissionLevel { get; set; } = Permissions.Level.VIEW;
    }
}
