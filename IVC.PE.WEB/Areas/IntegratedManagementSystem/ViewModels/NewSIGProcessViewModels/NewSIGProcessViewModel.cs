using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.EmployeeViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.NewSIGProcessViewModels
{
    public class NewSIGProcessViewModel
    {
        public Guid? Id { get; set; }
        public Guid ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }
        [Display(Name = "Jefe del Proceso", Prompt = "Jefe del Proceso")]
        public string UserId { get; set; }
        public string UserName { get; set; }
        [Display(Name = "Nombre del Proceso", Prompt = "Nombre del Proceso")]
        public string ProcessName { get; set; }
        [Display(Name = "Código del Proceso", Prompt = "Código del Proceso")]
        public string Code { get; set; }
    }
}
