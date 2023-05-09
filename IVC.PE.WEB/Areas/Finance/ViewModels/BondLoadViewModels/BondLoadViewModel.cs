using IVC.PE.CORE.Helpers;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondGuarantorViewModels;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondRenovationViewModels;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondTypeViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.EmployeeViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BankViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Finance.ViewModels.BondLoadViewModel
{
    public class BondLoadViewModel
    {


        public Guid? Id { get; set; }

        /* [Display(Name = "Proyecto", Prompt = "Proyecto")]

          public Guid ProjectId { get; set; }

          public ProjectViewModel Project { get; set; }
          */
        [Display(Name = "Garante", Prompt = "Garante")]
        public Guid BondGuarantorId { get; set; }
        public BondGuarantorViewModel BondGuarantor { get; set; }

        [Display(Name = "Título de presupuesto", Prompt = "Título de presupuesto")]
        public Guid BudgetTitleId { get; set; }
        public BudgetTitleViewModel BudgetTitle { get; set; }

        [Display(Name = "Tipo de Fianza", Prompt = "Tipo de Fianza")]
        public Guid BondTypeId { get; set; }
        public BondTypeViewModel BondType { get; set; }
   
        [Display(Name = "Entidad Financiera", Prompt = "Entidad Financiera")]
        public Guid BankId { get; set; }
        public BankViewModel Bank { get; set; }

        [Display(Name = "N° de Fianza", Prompt = "N° de Fianza")]
        public string BondNumber { get; set; }

        [Display(Name = "Renovación", Prompt = "Renovación")]
        public Guid BondRenovationId { get; set; }
        public BondRenovationViewModel BondRenovation { get; set; }

        [Display(Name = "Tipo Moneda", Prompt = "Tipo Moneda")]
        public string currencyType { get; set; }

        [Display(Name = "Monto", Prompt = "Monto")]
        public double PenAmmount { get; set; }

        [Display(Name = "Monto US$", Prompt = "Monto US$")]
        public double UsdAmmount { get; set; }

        [Display(Name = "Plazo", Prompt = "Plazo")]
        public int daysLimitTerm { get; set; }

        [Display(Name = "Fecha", Prompt = "Fecha")]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Contra Garantía", Prompt = "Contra Garantía")]
        public string guaranteeDesc { get; set; }

        [Display(Name = "Encargado", Prompt = "Encargado")]
        public Guid EmployeeId { get; set; }
        public EmployeeViewModel Employee { get; set; }


    }
}
