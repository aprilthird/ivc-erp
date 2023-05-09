using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels
{
    public class ProjectViewModel
    {
        [Display(Name = "Id")]
        public Guid? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Centro de Costo", Prompt = "Centro de Costo")]
        public string CostCenter { get; set; }

        [Display(Name = "Abreviación", Prompt = "Abreviación")]
        public string Abbreviation { get; set; }

        [Display(Name = "Ejecutor", Prompt = "Ejecutor")]
        public Guid? BusinessId { get; set; }

        [Display(Name="Logo", Prompt ="Logo")]
        public IFormFile LogoFile { get; set; }
        public Uri LogoUrl { get; set; }

        [Display(Name = "Firma Boletas", Prompt = "Firma Boletas")]
        public IFormFile InvoiceSignatureFile { get; set; }
        public Uri InvoiceSignatureUrl { get; set; }

        public BusinessViewModel Business {get; set;}

        [Display(Name ="Establecimiento Sunat", Prompt ="Establecimiento Sunat")]
        public string EstablishmentCode { get; set; }

        [Display(Name = "RUC", Prompt = "RUC")]
        public string RucCompany { get; set; }
    }
}
