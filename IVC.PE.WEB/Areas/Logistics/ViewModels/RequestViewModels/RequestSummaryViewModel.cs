using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.RequestViewModels
{
    public class RequestSummaryViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Proyecto", Prompt = "Proyecto")]
        public Guid ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }

        public int TotalOfRequest { get; set; }

        [Display(Name = "Prefijo", Prompt = "Prefijo")]
        public string CodePrefix { get; set; }
    }
}
