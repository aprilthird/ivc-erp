using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.InsuranceEntityViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTransportViewModels
{
    public class EquipmentMachineryTransportInsuranceFoldingViewModel
    {
        public Guid? Id { get; set; }


        public Guid EquipmentMachineryTransportId { get; set; }

        public EquipmentMachineryTransportViewModel EquipmentMachineryTransport { get; set; }

        [Display(Name = "Fecha de Inicio Seguro", Prompt = "Fecha de Inicio Seguro")]
        public string StartDateInsurance { get; set; } = DateTime.UtcNow.Date.ToShortDateString();

        [Display(Name = "Fecha de Fin Seguro", Prompt = "Fecha de Fin Seguro")]
        public string EndDateInsurance { get; set; } = DateTime.UtcNow.Date.ToShortDateString();




        [Display(Name = "# de Poliza", Prompt = "# de Poliza")]
        public string Number { get; set; }
        public Uri InsuranceFileUrl { get; set; }

        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile InsuranceFile { get; set; }

        [Display(Name = "Encargado(s)", Prompt = "Encargado(s)")]
        public IEnumerable<String> Responsibles { get; set; }

        public int OrderInsurance { get; set; }

        [Display(Name = "Aseguradora", Prompt = "Aseguradora")]
        public Guid? InsuranceEntityId { get; set; }

        public InsuranceEntityViewModel InsuranceEntity { get; set; }
    }
}