using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels
{
    public class PayrollParameterViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Proyecto", Prompt = "Proyecto")]
        public Guid ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }

        [Display(Name = "UIT", Prompt = "UIT")]
        public decimal UIT { get; set; }

        [Display(Name = "Sueldo Mínimo", Prompt = "Sueldo Mínimo")]
        public decimal MinimumWage { get; set; }

        [Display(Name = "T/C Dolar", Prompt = "T/C Dolar")]
        public decimal DollarExchangeRate { get; set; }

        [Display(Name = "Rem. Máxima Asegurable", Prompt = "Rem. Máxima Asegurable")]
        public decimal MaximumInsurableRemuneration { get; set; }

        [Display(Name = "Tasa SCTR", Prompt = "Tasa SCTR")]
        public decimal SCTRRate { get; set; }

        [Display(Name = "EsSalud +Vida", Prompt = "EsSalud +Vida")]
        public decimal EsSaludMasVidaCost { get; set; }

        [Display(Name = "Cuota Sindical", Prompt = "Cuota Sindical")]
        public decimal UnionFee { get; set; }

        [Display(Name = "SCTR Salud Fijo", Prompt = "SCTR Salud Fijo")]
        public decimal SCTRHealthFixed { get; set; }

        [Display(Name = "SCTR Pensión Fijo", Prompt = "SCTR Pensión Fijo")]
        public decimal SCTRPensionFixed { get; set; }
    }
}
