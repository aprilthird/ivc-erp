using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.AuthorizationTypeViewModels;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.AuthorizingEntityViewModels;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.PermissionRenovationViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.PermissionViewModels
{
    public class PermissionViewModel
    {
        public Guid? Id { get; set; }

        //[Display(Name = "Proyecto", Prompt = "Proyecto")]
        //public Guid ProjectId { get; set; }
        //public ProjectViewModel Project { get; set; }


        [Display(Name = "Fórmula", Prompt = "Fórmula")]
        public Guid ProjectFormulaId { get; set; }
        public ProjectFormulaViewModel ProjectFormula { get; set; }


        [Display(Name = "Vía Principal", Prompt = "Vía Principal")]
        public string PrincipalWay { get; set; }

        [Display(Name = "Desde", Prompt = "Desde")]
        public string From { get; set; }

        [Display(Name = "Hasta", Prompt = "Hasta")]
        public string To { get; set; }
        [Display(Name = "Tramo", Prompt = "Tramo")]
        public string Length { get; set; }
        [Display(Name = "Entidad que Autoriza", Prompt = "Entidad que Autoriza")]
        public Guid AuthorizingEntityId { get; set; }
        public AuthorizingEntityViewModel AuthorizingEntity { get; set; }

        [Display(Name = "Tipo de Autorización", Prompt = "Tipo de Autorización")]
        public Guid AuthorizationTypeId { get; set; }
        public AuthorizationTypeViewModel AuthorizationType { get; set; }

        public int NumberOfPermissions { get; set; }


        public PermissionRenovationViewModel PermissionRenovation { get; set; }
        public IEnumerable<PermissionRenovationViewModel> PermissionRenovations { get; set; }


        /*
        [Display(Name = "Renovación", Prompt = "Renovación")]
        public Guid BondRenovationId { get; set; }
        public BondRenovationViewModel BondRenovation { get; set; }

        [Display(Name = "Monto", Prompt = "Monto")]
        public double PenAmmount { get; set; }

        [Display(Name = "Monto US$", Prompt = "Monto US$")]
        public double UsdAmmount { get; set; }

        [Display(Name = "Tipo Moneda", Prompt = "Tipo Moneda")]
        public string currencyType { get; set; }

        [Display(Name = "Estado", Prompt = "Estado")]
        public int daysLimitTerm { get; set; }

        [Display(Name = "Fecha", Prompt = "Fecha")]
        [DisplayFormat(DataFormatString = "{dd-MM-YYYY}")]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public string CreateDateOnlyDate { get; set; }

        [Display(Name = "Fecha Fin", Prompt = "Fecha Fin")]
        [DisplayFormat(DataFormatString = "{dd-MM-YYYY}")]
        public DateTime EndDate { get; set; }
        public string EndDateOnlyDate { get; set; }

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

        */
    }
}
