using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels
{
    public class WorkFrontSewerGroupViewModel
    {
        public Guid WorkFrontId { get; set; }
        //public WorkFrontViewModel WorkFront { get; set; }

        public Guid SewerGroupId { get; set; }
        //public SewerGroupViewModel SewerGroup { get; set; }

        public string SewerGroupCode { get; set; }
    }
}
