using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.General
{
    public class WorkRoleItem
    {
        public Guid Id { get; set; }

        public WorkRole WorkRole { get; set; }

        public Guid WorkRoleId { get; set; }

        public WorkAreaItem WorkAreaItem { get; set; }

        public Guid WorkAreaItemId { get; set; }

        public int PermissionLevel { get; set; } = ConstantHelpers.Permissions.Level.VIEW;
    }
}
