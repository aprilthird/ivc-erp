using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeSoftViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTypeViewModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTransportViewModels;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryOperatorViewModels
{
    public class EquipmentMachineryOperatorViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Tipo de Equipo", Prompt = "Tipo de Equipo")]
        public Guid EquipmentMachineryTypeId { get; set; }
        public EquipmentMachineryTypeViewModel EquipmentMachineryType { get; set; }

        [Display(Name = "Nombre del Operador", Prompt = "Nombre del Operador")]
        public string OperatorName { get; set; }
        [Display(Name = "Número del Operador", Prompt = "Número del Operador")]
        public string PhoneOperator { get; set; }


        [Display(Name = "DNI del Operador", Prompt = "DNI del Operador")]
        public string DNIOperator { get; set; }

        [Display(Name = "DNI del Operador Externo", Prompt = "DNI del Operador Externo")]
        public string FromOtherDNI { get; set; }

        [Display(Name = "Nombre del Operador Externo", Prompt = "Nombre del Operador Externo")]
        public string FromOtherName { get; set; }
        [Display(Name = "Número del Operador Externo", Prompt = "Número del Operador Externo")]
        public string FromOtherPhone { get; set; }

        [Display(Name = "Tipo de Contratación", Prompt = "Tipo de Contratación")]
        public int HiringType { get; set; }
        public string HiringTypeDesc => ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.HIRING_TYPE[HiringType];


        
        public string ActualName{ get; set; }

        public string ActualDni { get; set; }


        //[Display(Name = "Tipo de Equipo", Prompt = "Tipo de Equipo")]
        //public Guid ConditionalType { get; set; }
        [Display(Name = "Obrero", Prompt = "Obrero")]
        public Guid? WorkerId { get; set; }

        public WorkerViewModel Worker { get; set; }
        [Display(Name = "Tipo de Equipo Menor", Prompt = "Tipo de Equipo Menor")]
        public Guid? EquipmentMachineryTypeSoftId { get; set; }

        public EquipmentMachineryTypeSoftViewModel EquipmentMachineryTypeSoft { get; set; }
        [Display(Name = "Tipo de Maquinaria", Prompt = "Tipo de Maquinaria")]
        public Guid? EquipmentMachineryTypeTypeId { get; set; }

        public EquipmentMachineryTypeTypeViewModel EquipmentMachineryTypeType { get; set; }


        [Display(Name = "Tipo de Transporte", Prompt = "Tipo de Maquinaria")]
        public Guid? EquipmentMachineryTypeTransportId { get; set; }

        public EquipmentMachineryTypeTransportViewModel EquipmentMachineryTypeTransport { get; set; }


        [Display(Name = "Fecha de inicio", Prompt = "Fecha de inicio")]
        public string StartDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();

        public Uri FileUrl { get; set; }

        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }     
    }
}
