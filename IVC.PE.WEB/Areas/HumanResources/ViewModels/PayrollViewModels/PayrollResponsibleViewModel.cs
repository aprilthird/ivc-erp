using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels
{
    public class PayrollResponsibleViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Proyecto", Prompt = "Proyecto")]
        public Guid? ProjectId { get; set; }
        public string ProjectFullName { get; set; }

        [Display(Name = "Autorización Tareo Semanal #1", Prompt = "Autorización Tareo Semanal #1")]
        public string Responsible1Id { get; set; }
        public string Responsible1FullName { get; set; }

        [Display(Name = "Autorización Tareo Semanal #2", Prompt = "Autorización Tareo Semanal #2")]
        public string Responsible2Id { get; set; }
        public string Responsible2FullName { get; set; }

        [Display(Name = "Autorización Planilla", Prompt = "Autorización Planilla")]
        public string Responsible3Id { get; set; }
        public string Responsible3FullName { get; set; }
    }
}
