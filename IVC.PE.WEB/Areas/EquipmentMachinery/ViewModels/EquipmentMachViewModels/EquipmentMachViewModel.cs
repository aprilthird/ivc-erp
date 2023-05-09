using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentProviderViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachViewModels
{
    public class EquipmentMachViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Proveedor", Prompt = "Proveedor")]
        public Guid EquipmentProviderId { get; set; }

        public EquipmentProviderViewModel EquipmentProvider { get; set; }
        [Display(Name = "Maquinaria", Prompt = "Maquinaria")]
        public Guid EquipmentProviderFoldingId { get; set; }
        public EquipmentProviderFoldingViewModel EquipmentProviderFolding { get; set; }

        [Display(Name = "Modelo", Prompt = "Modelo")]
        public string Model { get; set; }
        [Display(Name = "Marca", Prompt = "Marca")]
        public string Brand { get; set; }
        [Display(Name = "Potencia", Prompt = "Potencia")]
        public string Potency { get; set; }
        [Display(Name = "Placa", Prompt = "Placa")]
        public string Plate { get; set; }
        [Display(Name = "Año", Prompt = "Año")]
        public string Year { get; set; }

        [Display(Name = "# de Serie", Prompt = "# de Serie")]
        public string SerieNumber { get; set; }

        [Display(Name = "Fecha de Inicio", Prompt = "Fecha de Inicio")]
        public string StartDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();
        [Display(Name = "Cubicaje", Prompt = "Cubicaje")]
        public string Cubing { get; set; }

        [Display(Name = "Estado", Prompt = "Estado")]
        public int Status { get; set; }
        public string StatusDesc => ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.MACHINERY_STATUS[Status];
        [Display(Name = "Condición de Servicio", Prompt = "Condición de Servicio")]
        public int ServiceCondition { get; set; }
        public string ServiceConditionDesc => ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.MACHINERY_SERVICE_CONDITION[ServiceCondition];
        [Display(Name = "Precio Unitario", Prompt = "Precio Unitario")]
        public double UnitPrice { get; set; }

        public int InsuranceNumber { get; set; }

        public string LastStartDateInsurance { get; set; } = DateTime.UtcNow.Date.ToShortDateString();

        public string LastEndDateInsurance { get; set; } = DateTime.UtcNow.Date.ToShortDateString();

        public int LastValidityInsurance { get; set; }
    }
}
