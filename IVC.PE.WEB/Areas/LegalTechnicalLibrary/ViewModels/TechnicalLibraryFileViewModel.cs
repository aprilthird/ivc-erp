using IVC.PE.CORE.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.LegalTechnicalLibrary.ViewModels
{
    public class TechnicalLibraryFileViewModel
    {
        public Guid? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Título", Prompt = "Título")]
        public string Title { get; set; }

        public Uri FileUrl { get; set; }

        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }

        [Display(Name = "Tipo de Archivo", Prompt = "Tipo de Archivo")]
        public int FileType { get; set; }
    }

    public class SEDAPALTechnicalSpecificationsViewModel : TechnicalLibraryFileViewModel
    {
        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }

        [Display(Name = "Vigencia desde", Prompt = "Vigencia desde")]
        public string EffectiveDate { get; set; }
    }

    public class GeneralConstructionProceduresViewModel : TechnicalLibraryFileViewModel
    {
        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }

        [Display(Name = "Vigencia desde", Prompt = "Vigencia desde")]
        public string EffectiveDate { get; set; }
    }

    public class PeruvianTechnicalStandarsViewModel : TechnicalLibraryFileViewModel
    {
        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }

        [Display(Name = "Vigencia desde", Prompt = "Vigencia desde")]
        public string EffectiveDate { get; set; }
    }

    public class GeneralPolicyViewModel : TechnicalLibraryFileViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Fecha de Publicación", Prompt = "Fecha de Publicación")]
        public string PublicationDate { get; set; }
    }

    public class ManualTechnicalBookViewModel : TechnicalLibraryFileViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Autor", Prompt = "Autor")]
        public string Author { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Fecha de Publicación", Prompt = "Fecha de Publicación")]
        public string PublicationDate { get; set; }
    }

    public class SupplierCatalogViewModel : TechnicalLibraryFileViewModel
    {
        [Display(Name = "Proveedor", Prompt = "Proveedor")]

        public Guid? SupplierGuid { get; set; }

        public SupplierViewModel SupplierViewModel { get; set; }
    }
}
