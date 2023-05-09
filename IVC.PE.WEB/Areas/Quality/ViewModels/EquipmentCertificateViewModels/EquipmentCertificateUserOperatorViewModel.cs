using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.ViewModels.EquipmentCertificateViewModels
{
    public class EquipmentCertificateUserOperatorViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Operador", Prompt = "Operador")]
        public string Operator { get; set; }

        [Display(Name = "Proyecto", Prompt = "Proyecto")]
        public Guid ProjectId { get; set; }

        public ProjectViewModel Project { get; set; }
    }
}
