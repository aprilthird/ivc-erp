using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkPositionViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BankViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels
{
    public class WorkerViewModel
    {
        public Guid? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Primer Nombre", Prompt = "Primer Nombre")]
        public string Name { get; set; }

        [Display(Name = "Segundo Nombre", Prompt = "Segundo Nombre")]
        public string MiddleName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Apellido Paterno", Prompt = "Apellido Paterno")]
        public string PaternalSurname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Apellido Materno", Prompt = "Apellido Materno")]
        public string MaternalSurname { get; set; }

        public string FullName => PaternalSurname + " " + MaternalSurname + " " + Name;

        [Display(Name = "Tipo de Documento", Prompt = "Tipo de Documento")]
        public int DocumentType { get; set; }
        public string DocumentTypeStr => ConstantHelpers.DocumentType.SVALUES[DocumentType];

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Documento", Prompt = "Documento")]
        public string Document { get; set; }

        [Display(Name = "Fecha de Nacimiento", Prompt = "Fecha de Nacimiento")]
        public string BirthDate { get; set; }

        [Display(Name = "Teléfono", Prompt = "Teléfono")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Correo Electrónico", Prompt = "Correo Electrónico")]
        public string Email { get; set; }

        [Display(Name = "Foto", Prompt = "Foto")]
        public IFormFile PhotoFile { get; set; }
        public Uri PhotoUrl { get; set; }

        public Guid? WorkPeriodId { get; set; }
        public WorkerWorkPeriodViewModel WorkPeriod { get; set; }



        public string CeaseDateStr { get; set; }
        public bool IsActive { get; set; }


        //Campos para Fotocheck
        public string CategoryStr { get; set; }
        public Uri LogoUrl { get; set; }
        public string ProjectPhoneNumber { get; set; }
        public string ProjectAddress { get; set; }

        //Campos banco
        [Display(Name = "Banco", Prompt = "Banco")]
        public Guid? BankId { get; set; }
        public BankViewModel Bank { get; set; }
        [Display(Name = "Cta. Bancaria", Prompt = "Cta. Bancaria")]
        public string BankAccount { get; set; }

        [Display(Name = "Género", Prompt = "Género")]
        public int Gender { get; set; }
        public string GenderStr => ConstantHelpers.Worker.Gender.VALUES[Gender];
    }
}
