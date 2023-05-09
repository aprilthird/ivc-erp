using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.FuelProviderViewModels
{
    public class FuelProviderFoldingViewModel
    {
        public Guid? Id { get; set; }

        public Guid FuelProviderId { get; set; }

        public FuelProviderViewModel FuelProvider { get; set; }
        [Display(Name = "Placa de Cisterna", Prompt = "Placa de Cisterna")]
        public string CisternPlate { get; set; }


    }
}
