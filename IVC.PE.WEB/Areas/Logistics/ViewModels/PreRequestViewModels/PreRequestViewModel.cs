using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.RequestViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.PreRequestViewModels
{
    public class PreRequestViewModel
    {
        public Guid? Id { get; set; }

        public Guid? ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }

        //[Display(Name = "Familia", Prompt = "Familia")]
        //public Guid SupplyFamilyId { get; set; }
        //public SupplyFamilyViewModel SupplyFamily { get; set; }

        [Display(Name = "Fórmula", Prompt = "Fórmula")]
        public Guid ProjectFormulaId { get; set; }

        public ProjectFormulaViewModel ProjectFormula { get; set; }

        public int CorrelativeCode { get; set; }
        public string CorrelativeCodeStr { get; set; }

        public int OrderStatus { get; set; }

        [Display(Name = "Presupuesto", Prompt = "Presupuesto")]
        public Guid BudgetTitleId { get; set; }
        public BudgetTitleViewModel BudgetTitle { get; set; }

        [Display(Name = "Tipo", Prompt = "Tipo")]
        public int RequestType { get; set; }
        public string RequestTypeStr => ConstantHelpers.Logistics.RequestOrder.Type.VALUES[RequestType];

        public string IssueDate { get; set; }

        [Display(Name = "Fecha de Entrega", Prompt = "Fecha de Entrega")]
        public string DeliveryDate { get; set; }

        public int AttentionStatus { get; set; }

        //[Display(Name = "Solicita", Prompt = "Solicita")]
        //public List<string> PreRequestUserIds { get; set; }


        public List<Uri> RequestFiles { get; set; }
        public bool HasFiles { get; set; }
        public List<string> RequestFileNames { get; set; }

        public List<PreRequestItemViewModel> PreRequestItems { get; set; }
        public string RequestUsernames { get; set; }

        public string Observations { get; set; }

        public string IssuedUserId { get; set; }
        public string IssuedUserName { get; set; }
        public string ApproveDate { get; set; }
    }

    public class PreRequestObsViewModel
    {
        [DataType(DataType.Upload)]
        [Display(Name = "Adjuntar archivos", Prompt = "Adjuntar archivos")]
        public IEnumerable<IFormFile> PreRequestFiles { get; set; }

        [Display(Name = "Observaciones del Pre-Requerimiento (Opcional)", Prompt = "Observaciones del Pre-Requerimiento (Opcional)")]
        public string Observations { get; set; }
    }

    public class PreRequestInRequestItemsViewModel
    {
        public int CorrelativeCode { get; set; }
        public string CorrelativeCodeStr { get; set; }

    }
}
