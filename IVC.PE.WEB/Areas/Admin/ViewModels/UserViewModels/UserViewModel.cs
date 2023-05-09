using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkAreaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkPositionViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkRoleViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.ViewModels.UserViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

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

        [Display(Name = "Área de Trabajo", Prompt = "Área de Trabajo")]
        public Guid? WorkAreaId { get; set; }

        public WorkAreaViewModel WorkAreaEntity { get; set; }
        public int WorkArea { get; set; }

        [Display(Name = "Rol de Trabajo", Prompt = "Rol de Trabajo")]
        public Guid? WorkRoleId { get; set; }

        public WorkRoleViewModel WorkRole { get; set; }

        [Display(Name = "Cargo en Obra", Prompt = "Cargo en Obra")]
        public Guid? WorkPositionId { get; set; }

        public WorkPositionViewModel WorkPosition { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [EmailAddress(ErrorMessage = ConstantHelpers.Messages.Validation.EMAIL_ADDRESS)]
        [Display(Name = "Correo Electrónico (Usuario)", Prompt = "Correo Electrónico")]
        public string Email { get; set; }

        [Display(Name = "Teléfono", Prompt = "Teléfono")]
        public string PhoneNumber { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&._-]).{6,}$", ErrorMessage = "La contraseña debe contener al menos un dígito, una mayúscula, minúscula, un caracter especial y ser de 6 caracteres de largo como mínimo.")]
        [Display(Name = "Contraseña", Prompt = "Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = ConstantHelpers.Messages.Validation.COMPARE)]
        [Display(Name = "Repetir Contraseña", Prompt = "Repetir Contraseña")]
        [DataType(DataType.Password)]
        public string RepeatPassword { get; set; }

        public string FullName => $"{PaternalSurname} {MaternalSurname}, {Name}{(string.IsNullOrEmpty(MiddleName) ? string.Empty : $" {MiddleName}")}";
        [Display(Name = "Nombre")]

        public string AuxFullName { get; set; }

        public string RawFullName => $"{Name}{(string.IsNullOrEmpty(MiddleName) ? string.Empty : $" {MiddleName}")} {PaternalSurname} {MaternalSurname}";

        [Display(Name = "Pertenece a Oficina Central")]
        public bool BelongsToMainOffice { get; set; }

        [Display(Name = "Roles", Prompt = "Roles")]
        public IEnumerable<string> RoleIds { get; set; }

        public IEnumerable<string> RoleNames { get; set; }

        public string StringRoleNames { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Proyectos", Prompt = "Proyectos")]
        public IEnumerable<Guid> ProjectIds { get; set; }

        public string StringProjectNames { get; set; }

        [Display(Name = "Firma", Prompt = "Firma")]
        public IFormFile SignatureFile { get; set; }
        public Uri SignatureUrl { get; set; }
    }
}
