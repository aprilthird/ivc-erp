using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.EmployeeViewModels
{
    public class EmployeeViewModel
    {
        public Guid Id { get; set; }

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

        [Display(Name = "Tipo de Documento", Prompt = "Tipo de Documento")]
        public int DocumentType { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Documento", Prompt = "Documento")]
        public string Document { get; set; }

        public string DocumentTypeStr => ConstantHelpers.DocumentType.SVALUES[DocumentType];

        [Display(Name = "Fecha de Nacimiento", Prompt = "Fecha de Nacimiento")]
        public string BirthDate { get; set; }

        [Display(Name = "Fecha de Ingreso", Prompt = "Fecha de Ingreso")]
        public string EntryDate { get; set; }

        [Display(Name = "Teléfono", Prompt = "Teléfono")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Área de Trabajo", Prompt = "Área de Trabajo")]
        public int WorkArea { get; set; }

        [Display(Name = "Cargo Contractual", Prompt = "Cargo Contractual")]
        public Guid EntryPositionId { get; set; }

        public WorkPositionViewModel EntryPosition { get; set; }

        [Display(Name = "Cargo en Obra", Prompt = "Cargo en Obra")]
        public Guid? CurrentPositionId { get; set; }
        
        public WorkPositionViewModel CurrentPosition { get; set; }

        [Display(Name = "Correo Electrónico", Prompt = "Correo Electrónico")]
        public string Email { get; set; }

        [Display(Name = "AFP", Prompt = "AFP")]
        public Guid? PensionFundAdministratorId { get; set; }

        [Display(Name = "CUSSP", Prompt = "CUSSP")]
        public string PensionFundUniqueIdentificationCode { get; set; }

        public PensionFundAdministratorViewModel PensionFundAdministrator { get; set; }
    }
}
