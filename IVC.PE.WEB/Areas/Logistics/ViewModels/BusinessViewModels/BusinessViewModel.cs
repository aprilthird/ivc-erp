using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BankViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessViewModels
{
    public class BusinessViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Tipo de Sociedad", Prompt = "Tipo de Sociedad")]
        public int Type { get; set; }
        public double? DefaultParticipation { get; set; }
        [Display(Name = "Empresa 1", Prompt = "Empresa 1")]
        public string BusinessConsortium1 { get; set; }
        [Display(Name = "Empresa 2", Prompt = "Empresa 2")]
        public string BusinessConsortium2 { get; set; }
        [Display(Name = "Empresa 3", Prompt = "Empresa 3")]
        public string BusinessConsortium3 { get; set; }
        [Display(Name = "Empresa 4", Prompt = "Empresa 4")]
        public string BusinessConsortium4 { get; set; }
        [Display(Name = "Empresa 5", Prompt = "Empresa 5")]
        public string BusinessConsortium5 { get; set; }

       

        [Display(Name = "Razón Social", Prompt = "Razón Social")]
        public string BusinessName { get; set; }

        [Display(Name = "Nombre Comercial", Prompt = "Nombre Comercial")]
        public string Tradename { get; set; }

        [Display(Name = "RUC", Prompt = "RUC")]
        public string RUC { get; set; }

        
        [Display(Name = "Dirección", Prompt = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Teléfono", Prompt = "Teléfono")]
        public string PhoneNumber { get; set; }



        [Display(Name = "Nombre Completo", Prompt = "Nombre Completo")]
        public string CollectionAreaContactName { get; set; }

        [Display(Name = "Correo ELectrónico", Prompt = "Correo ELectrónico")]
        public string CollectionAreaEmail { get; set; }

        [Display(Name = "Teléfono", Prompt = "Teléfono")]
        public string CollectionAreaPhoneNumber { get; set; }

        public Uri RucUrl { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Archivo Ruc", Prompt = "Archivo Ruc")]
        public IFormFile FileRuc { get; set; }

        public Uri TestimonyUrl { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Archivo Testimonio", Prompt = "Archivo Testimonio")]
        public IFormFile FileTestimony { get; set; }

        [Display(Name = "Fecha de creación", Prompt = "Fecha de creación")]
        public string CreateDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();
    }
}
