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
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Finance.ViewModels.BondReportViewModel
{
    public class BondReportViewModel
    {

        public Guid? Id { get; set; }

        [Display(Name = "Proyecto", Prompt = "Proyecto")]
        public Guid ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }


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

        [Display(Name = "Monto", Prompt = "Monto")]
        public double PenAmmount { get; set; }

        [Display(Name = "Monto US$", Prompt = "Monto US$")]
        public double UsdAmmount { get; set; }

        [Display(Name = "Tipo Moneda", Prompt = "Tipo Moneda")]
        public string currencyType { get; set; }

        [Display(Name = "Plazo", Prompt = "Plazo")]
        public int daysLimitTerm { get; set; }

        [Display(Name = "Fecha", Prompt = "Fecha")]
        [DisplayFormat(DataFormatString = "{dd-MM-YYYY}")]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Fecha Fin", Prompt = "Fecha Fin")]
        [DisplayFormat(DataFormatString = "{dd-MM-YYYY}")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Plazo", Prompt = "Plazo")]
        public string daysLimit => (EndDate.Date - CreateDate.Date).TotalDays.ToString();

        [Display(Name = "Inicio de vigencia", Prompt = "Inicio de vigencia")]
        public string DateInitial => $"{CreateDate.Date.ToShortDateString()}";

        [Display(Name = "Vencimiento de vigencia", Prompt = "Vencimiento de vigencia")]
        public string DateEnd => $"{EndDate.Date.ToShortDateString()}";

        [Display(Name = "Dias al vencimiento", Prompt = "Dias al vencimiento")]
        public string daysToEnd => (EndDate.Date - DateTime.UtcNow.Date).TotalDays.ToString();

        [Display(Name = "Contra Garantía", Prompt = "Contra Garantía")]
        public string guaranteeDesc { get; set; }
        public string penAmmount2 => String.Format(new CultureInfo("es-PE"), "{0:C}", PenAmmount);

        [Display(Name = "Contra Garantía", Prompt = "Contra Garantía")]
        public string guaranteeDesc2 { get; set; }

        [Display(Name = "Encargado", Prompt = "Encargado")]
        public Guid EmployeeId { get; set; }
        public EmployeeViewModel Employee { get; set; }
        [Display(Name = "Centro de Costo", Prompt = "Centro de Costo")]
        public string AgrupedBy { get; set; }
        public string ExpiresInFewDays { get; set; }

        public string IsActive { get; set; }

        public bool IsEnabled => Convert.ToInt32(daysToEnd) >= 0;

        public bool IsExpired => Convert.ToInt32(daysToEnd) < 0;

        public bool isClosed => daysLimitTerm == 1;


        public string ClosedWithBonds { get; set; }





    }
}
