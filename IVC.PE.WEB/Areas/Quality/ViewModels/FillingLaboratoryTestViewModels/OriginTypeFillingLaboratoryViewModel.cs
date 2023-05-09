using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.ViewModels.FillingLaboratoryTestViewModels
{
    public class OriginTypeFillingLaboratoryViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Procedencia", Prompt = "Procedencia")]
        public string OriginTypeFLName { get; set; }
        [Display(Name = "Proyecto", Prompt = "Proyecto")]
        public Guid ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }
    }
}
