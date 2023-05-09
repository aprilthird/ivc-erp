using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryOperatorViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.FuelProviderViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryFuelTransportPartViewModels
{
    public class EquipmentMachineryFuelTransportPartFoldingViewModel
    {
        public Guid? Id { get; set; }

        public Guid EquipmentMachineryFuelTransportPartId { get; set; }

        public EquipmentMachineryFuelTransportPartViewModel EquipmentMachineryFuelTransportPart { get; set; }
        [Display(Name = "# de parte", Prompt = "# de parte")]
        public string PartNumber { get; set; }

        [Display(Name = "Distribuidor de Combustible", Prompt = "Distribuidor de Combustible")]
        public Guid FuelProviderId { get; set; }

        public FuelProviderViewModel FuelProvider { get; set; }



        [Display(Name = "Placa Cisterna", Prompt = "Placa Cisterna")]
        public Guid FuelProviderFoldingId { get; set; }

        public FuelProviderFoldingViewModel FuelProviderFolding { get; set; }

        [Display(Name = "Precio (S/ x gln)", Prompt = "Precio (S/ x gln)")]
        public Guid? FuelProviderPriceFoldingId { get; set; }

        public FuelProviderPriceFoldingViewModel FuelProviderPriceFolding { get; set; }



        [Display(Name = "Fecha", Prompt = "Fecha")]
        public string PartDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();
        [Display(Name = "Hora", Prompt = "Hora")]
        public string PartHour { get; set; }
        [Display(Name = "Operador", Prompt = "Operador")]
        public Guid EquipmentMachineryOperatorId { get; set; }
        public EquipmentMachineryOperatorViewModel EquipmentMachineryOperator { get; set; }
        [Display(Name = "Kilometraje Carga", Prompt = "Kilometraje Carga")]

        public double Mileage { get; set; }
        [Display(Name = "Consumo (gln)", Prompt = "Consumo (gln)")]
        public double Gallon { get; set; }

        public int Order { get; set; }
    }
}
