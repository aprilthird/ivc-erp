using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.ViewModels.EquipmentCertificateViewModels
{
    public class EquipmentCertificateOwnerViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Propietario", Prompt = "Propietario")]
        public string OwnerName { get; set; }

        [Display(Name = "Proyecto", Prompt = "Proyecto")]
        public Guid ProjectId { get; set; }

        public ProjectViewModel Project { get; set; }
    }
}
