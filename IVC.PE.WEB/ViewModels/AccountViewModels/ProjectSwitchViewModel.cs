using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.ViewModels.AccountViewModels
{
    public class ProjectSwitchViewModel
    {
        public Guid ProjectId { get; set; }

        public IEnumerable<ProjectViewModel> Projects { get; set; }
    }
}
