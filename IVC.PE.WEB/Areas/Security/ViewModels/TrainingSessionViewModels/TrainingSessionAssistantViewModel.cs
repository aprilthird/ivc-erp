using IVC.PE.WEB.Areas.HumanResources.ViewModels.EmployeeViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels;
using IVC.PE.WEB.Areas.Security.ViewModels.TrainingResultStatusViewModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace IVC.PE.WEB.Areas.Security.ViewModels.TrainingSessionViewModels
{
    public class TrainingSessionAssistantViewModel
    {
        public WorkerViewModel Worker { get; set; }

        [Display(Name = "Obrero", Prompt = "Obrero")]
        public Guid? WorkerId { get; set; }

        public EmployeeViewModel Employee { get; set; }

        [Display(Name = "Empleado", Prompt = "Empleado")]
        public Guid? EmployeeId { get; set; }

        [Display(Name = "Resultado", Prompt = "Resultado")]
        public Guid TrainingResultStatusId { get; set; }

        public TrainingResultStatusViewModel TrainingResultStatus { get; set; }

        [Display(Name = "Observación", Prompt = "Observación")]
        public string Observation { get; set; }
    }
}
