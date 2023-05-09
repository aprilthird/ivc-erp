using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.PermissionRenovationViewModels
{
    public class PermissionRenovationApplicationUserViewModel
    {
        public Guid? Id { get; set; }

        public Guid? PermissionRenovationId { get; set; }
        public PermissionRenovationViewModel PermissionRenovationViewModel { get; set; }

        public String UserId { get; set; }
    }
}
