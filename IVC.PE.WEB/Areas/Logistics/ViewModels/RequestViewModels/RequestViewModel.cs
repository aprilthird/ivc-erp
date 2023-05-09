using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.WarehouseViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.RequestViewModels
{
    public class RequestViewModel
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

        public string ReviewDate { get; set; }

        public string ApproveDate { get; set; }

        [Display(Name = "Fecha de Entrega", Prompt = "Fecha de Entrega")]
        public string DeliveryDate { get; set; }

        [Display(Name = "Lugar de Entrega", Prompt = "Lugar de Entrega")]
        public Guid? WarehouseId { get; set; }
        public WarehouseViewModel Warehouse { get; set; }

        //public Guid? RequestDeliveryPlaceId { get; set; }
        //public RequestDeliveryPlaceViewModel RequestDeliveryPlace { get; set; }


        public int AttentionStatus { get; set; }

        //[Display(Name = "Solicita", Prompt = "Solicita")]
        //public List<string> RequestUserIds { get; set; }


        public List<Uri> RequestFiles { get; set; }
        public bool HasFiles { get; set; }
        public List<string> RequestFileNames { get; set; }

        public List<RequestItemViewModel> RequestItems { get; set; }
        public string RequestUsernames { get; set; }

        public string Observations { get; set; }

        public bool QualityCertificate { get; set; }
        public bool Blueprint { get; set; }
        public bool TechnicalInformation { get; set; }
        public bool CalibrationCertificate { get; set; }
        public bool Catalog { get; set; }
        public bool Other { get; set; }
        public string OtherDescription { get; set; }

        public string IssuedUserId { get; set; }
        public string IssuedUserName { get; set; }

        public string PreRequestNames { get; set; }
        public string GroupNames { get; set; }
    }

    public class RequestObsViewModel
    {
        [DataType(DataType.Upload)]
        [Display(Name = "Adjuntar archivos", Prompt = "Adjuntar archivos")]
        public IEnumerable<IFormFile> RequestFiles { get; set; }

        [Display(Name = "Observaciones del Requerimiento (Opcional)", Prompt = "Observaciones del Requerimiento (Opcional)")]
        public string Observations { get; set; }
        [Display(Name = "Certificados de Calidad", Prompt = "Certificados de Calidad")]
        public bool QualityCertificate { get; set; }
        [Display(Name = "Planos", Prompt = "Planos")]
        public bool Blueprint { get; set; }
        [Display(Name = "Información Técnica", Prompt = "Información Técnica")]
        public bool TechnicalInformation { get; set; }
        [Display(Name = "Certificados de Calibración", Prompt = "Certificados de Calibración")]
        public bool CalibrationCertificate { get; set; }
        [Display(Name = "Catálogos", Prompt = "Catálogos")]
        public bool Catalog { get; set; }
        [Display(Name = "Otros", Prompt = "Otros")]
        public bool Other { get; set; }
        [Display(Name = "Otros (Descripción)", Prompt = "Otros (Descripción)")]
        public string OtherDescription { get; set; }
    }

    public class ExistingRequestViewModel
    {
        [Display(Name = "Pre-Requerimientos", Prompt = "Pre-Requerimientos")]
        public List<Guid> PreRequestIds { get; set; }

        [Display(Name = "Lugar de Entrega", Prompt = "Lugar de Entrega")]
        public Guid WarehouseId { get; set; }

        public Guid RequestDeliveryPlaceId { get; set; }

        public IEnumerable<string> Items { get; set; }
    }

    public class RequestInRequestItemsViewModel
    {
        public int CorrelativeCode { get; set; }
        public string CorrelativeCodeStr { get; set; }

    }

    public class SendRequestObsViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Para")]
        public string Email { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        public string Obs { get; set; }

    }
}
