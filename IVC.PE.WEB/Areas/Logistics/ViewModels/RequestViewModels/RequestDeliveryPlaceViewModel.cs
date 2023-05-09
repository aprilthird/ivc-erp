using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.RequestViewModels
{
    public class RequestDeliveryPlaceViewModel
    {
        public Guid? Id { get; set; }

        public Guid ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }

        [Display(Name ="Descripción", Prompt ="Descripción")]
        public string Description { get; set; }
    }
}
