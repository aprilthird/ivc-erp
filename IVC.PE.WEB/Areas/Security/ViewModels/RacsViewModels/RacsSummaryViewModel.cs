using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Security.ViewModels.RacsViewModels
{
    public class RacsSummaryViewModel
    {
        public Guid? Id { get; set; }
        public Guid ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }
        public string RacsCode { get; set; }
        public int RacsCount { get; set; }
        public string VersionCode { get; set; }
    }
}
