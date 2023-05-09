using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.LogisticResponsibleViewModels
{
    public class LogisticResponsibleViewModel
    {
        [Display(Name = "Proyecto", Prompt = "Proyecto")]
        public Guid? ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }

        [Display(Name = "Revisión Requerimientos", Prompt = "Revisión Requerimiento")]
        public IEnumerable<string> RequestReviewIds { get; set; }
        public string RequestReviewNames { get; set; }

        [Display(Name = "Autorización Requerimientos", Prompt = "Autorización Requerimiento")]
        public IEnumerable<string> RequestAuthIds { get; set; }
        public string RequestAuthNames { get; set; }

        [Display(Name = "Informar Aprobación Requerimientos", Prompt = "Informar Aprobación")]
        public IEnumerable<string> RequestOkIds { get; set; }
        public string RequestOkNames { get; set; }

        [Display(Name = "Informar Rechazo Requerimientos", Prompt = "Informar Rechazo")]
        public IEnumerable<string> RequestFailIds { get; set; }
        public string RequestFailNames { get; set; }

        [Display(Name = "Revisión Órdenes", Prompt = "Revisión Órdenes")]
        public IEnumerable<string> OrderReviewIds { get; set; }
        public string OrderReviewNames { get; set; }

        [Display(Name = "Autorización Órdenes", Prompt = "Autorización Órdenes")]
        public IEnumerable<string> OrderAuthIds { get; set; }
        public string OrderAuthNames { get; set; }

        [Display(Name = "Informar Aprobación Órdenes", Prompt = "Informar Aprobación")]
        public IEnumerable<string> OrderOkIds { get; set; }
        public string OrderOkNames { get; set; }

        [Display(Name = "Informar Rechazo Órdenes", Prompt = "Informar Rechazo")]
        public IEnumerable<string> OrderFailIds { get; set; }
        public string OrderFailNames { get; set; }

        [Display(Name = "Autorización Pre-Requerimientos", Prompt = "Autorización Pre-Requerimiento")]
        public IEnumerable<string> PreRequestAuthIds { get; set; }
        public string PreRequestAuthNames { get; set; }

        [Display(Name = "Autorización Secundaria Pre-Requerimientos", Prompt = "Autorización Secundaria Pre-Requerimiento")]
        public IEnumerable<string> SecondaryPreRequestAuthIds { get; set; }
        public string SecondaryPreRequestAuthNames { get; set; }

        [Display(Name = "Informar Aprobación Pre-Requerimientos", Prompt = "Informar Aprobación")]
        public IEnumerable<string> PreRequestOkIds { get; set; }
        public string PreRequestOkNames { get; set; }

        [Display(Name = "Informar Rechazo Pre-Requerimientos", Prompt = "Informar Rechazo")]
        public IEnumerable<string> PreRequestFailIds { get; set; }
        public string PreRequestFailNames { get; set; }
    }
}
