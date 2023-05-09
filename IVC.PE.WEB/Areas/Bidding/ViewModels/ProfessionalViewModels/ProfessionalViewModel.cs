using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Bidding.ViewModels.ProfessionViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.ViewModels.ProfessionalsViewModels
{
    public class ProfessionalViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Experiencia Acumulada Total (Días)", Prompt = "Experiencia Acumulada Total (Días)")]
        public int Dif { get; set; }
        [Display(Name = "Experiencia Acumulada (Años)", Prompt = "Experiencia Acumulada (Años)")]
        public int Years { get; set; }
        [Display(Name = "Experiencia Acumulada (Meses)", Prompt = "Experiencia Acumulada (Meses)")]
        public int Months { get; set; }
        [Display(Name = "Experiencia Acumulada (Días)", Prompt = "Experiencia Acumulada (Días)")]
        public int Days { get; set; }

        [Display(Name = "Primer Nombre", Prompt = "Primer Nombre")]
        public string Name { get; set; }

        [Display(Name = "Segundo Nombre", Prompt = "Segundo Nombre")]
        public string MiddleName { get; set; }
        
        [Display(Name = "Apellido Paterno", Prompt = "Apellido Paterno")]
        public string PaternalSurname { get; set; }

        
        [Display(Name = "Apellido Materno", Prompt = "Apellido Materno")]
        public string MaternalSurname { get; set; }

        public string FullName => $"{PaternalSurname} {MaternalSurname}, {Name}{(string.IsNullOrEmpty(MiddleName) ? string.Empty : $" {MiddleName}")}";
        [Display(Name = "Domicilio", Prompt = "Domicilio")]
        public string Address { get; set; }
        [Display(Name = "Universidad o Centro de Estudios", Prompt = "Universidad o Centro de Estudios")]
        public string University { get; set; }
        [Display(Name = "Nacionalidad", Prompt = "Nacionalidad")]
        public string Nacionality { get; set; }

        [Display(Name = "Documento", Prompt = "Documento")]
        public string Document { get; set; }

        [Display(Name = "Teléfono", Prompt = "Teléfono")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Correo Electrónico", Prompt = "Correo Electrónico")]
        public string Email { get; set; }

        [Display(Name = "Colegiatura", Prompt = "Colegiatura")]
        public string CIPNumber { get; set; }

        [Display(Name = "Profesión", Prompt = "Profesión")]
        public Guid ProfessionId { get; set; }
        public ProfessionViewModel Profession { get; set; }

        [Display(Name = "Fecha de nacimiento", Prompt = "Fecha de nacimiento")]
        public string BirthDateStr { get; set; } = DateTime.UtcNow.Date.ToShortDateString();

        [Display(Name = "Fecha de Expedición del Título", Prompt = "Fecha de Expedición del Título")]
        public string StartTitleStr { get; set; } = DateTime.UtcNow.Date.ToShortDateString();
        [Display(Name = "Fecha de Colegiatura", Prompt = "Fecha de Colegiatura")]
        public string CipDateStr { get; set; } = DateTime.UtcNow.Date.ToShortDateString();
       
        [Display(Name = "¿Tiene Validación con Sunedu?", Prompt = "¿Tiene Validación con Sunedu?")]
        public bool ValidationSunedu { get; set; }
        [Display(Name = "¿Tiene CertiAdulto?", Prompt = "¿Tiene CertiAdulto?")]
        public bool CertiAdult { get; set; }

        public Uri DniUrl { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "DNI ADJUNTO", Prompt = "Archivo")]
        public IFormFile FileDni { get; set; }

        public Uri TitleUrl { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Título ADJUNTO", Prompt = "Archivo")]
        public IFormFile FileTitle { get; set; }
        public Uri CipUrl { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Colegiatura ADJUNTO", Prompt = "Archivo")]
        public IFormFile FileCip { get; set; }


        public Uri CertiAdultUrl { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "CertiAdulto ADJUNTO", Prompt = "Archivo")]
        public IFormFile FileCerti { get; set; }

        public Uri CapacitationUrl { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Capacitación ADJUNTO", Prompt = "Archivo")]
        public IFormFile FileCapacitation { get; set; }





    }
}
