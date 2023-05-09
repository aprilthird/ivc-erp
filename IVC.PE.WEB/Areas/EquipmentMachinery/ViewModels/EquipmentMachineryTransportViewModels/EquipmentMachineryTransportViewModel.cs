using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentProviderViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTransportViewModels
{
    public class EquipmentMachineryTransportViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Proveedor de Equipos", Prompt = "Proveedor de Equipos")]
        public Guid EquipmentProviderId { get; set; }

        public EquipmentProviderViewModel EquipmentProvider { get; set; }

        [Display(Name = "Equipos Transporte", Prompt = "Equipos Transporte")]
        public Guid EquipmentProviderFoldingId { get; set; }

        public EquipmentProviderFoldingViewModel EquipmentProviderFolding { get; set; }

        //[Display(Name = "Equipos Menores", Prompt = "Equipos Menores")]
        //public Guid EquipmentMachineryTypeSoftId { get; set; }

        //public EquipmentMachineryTypeSoftViewModel EquipmentMachineryTypeSoft { get; set; }
        [Display(Name = "Marca del Equipo", Prompt = "Marca del Equipo")]
        public string Brand { get; set; }

        [Display(Name = "Modelo del Equipo", Prompt = "Modelo del Equipo")]
        public string Model { get; set; }


        [Display(Name = "Año del Equipo", Prompt = "Año del Equipo")]
        public string EquipmentYear { get; set; }
        [Display(Name = "Placa del Equipo", Prompt = "Placa del Equipo")]
        public string EquipmentPlate { get; set; }

        [Display(Name = "# de Serie", Prompt = "# de Serie")]
        public string EquipmentSerie { get; set; }

        [Display(Name = "Fecha de Inicio", Prompt = "Fecha de Inicio")]
        public string StartDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();


        public string LastStartDateInsurance { get; set; } = DateTime.UtcNow.Date.ToShortDateString();


        public string LastEndDateInsurance { get; set; } = DateTime.UtcNow.Date.ToShortDateString();

        public int LastInsuranceName { get; set; }

        public string LastInsuranceNumber { get; set; }

        public string LastStartDateSoat { get; set; } = DateTime.UtcNow.Date.ToShortDateString();


        public string LastEndDateSoat { get; set; } = DateTime.UtcNow.Date.ToShortDateString();

        public int LastValiditySoat { get; set; }

        public string LastStartDateTec { get; set; } = DateTime.UtcNow.Date.ToShortDateString();


        public string LastEndDateTec { get; set; } = DateTime.UtcNow.Date.ToShortDateString();

        public int LastValidityTec { get; set; }

        public int InsuranceNumber { get; set; }

        public int SoatNumber { get; set; }

        public int TecNumber { get; set; }

        [Display(Name = "Estado", Prompt = "Estado")]
        public int Status { get; set; }
        public string StatusDesc => ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.MACHINERY_STATUS[Status];
        [Display(Name = "Condición de Servicio", Prompt = "Condición de Servicio")]
        public int ServiceCondition { get; set; }
        public string ServiceConditionDesc => ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.MACHINERY_SERVICE_CONDITION[ServiceCondition];
        [Display(Name = "Precio Unitario (S/ x día)", Prompt = "Precio Unitario (S/ x día)")]
        public double UnitPrice { get; set; }
    }
}
