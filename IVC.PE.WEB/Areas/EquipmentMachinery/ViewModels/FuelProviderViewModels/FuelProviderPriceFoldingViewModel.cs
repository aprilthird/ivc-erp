using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.FuelProviderViewModels
{
    public class FuelProviderPriceFoldingViewModel
    {
        public Guid? Id { get; set; }

        public Guid FuelProviderId { get; set; }

        public FuelProviderViewModel FuelProvider { get; set; }
        [Display(Name = "Precio", Prompt = "Precio")]
        public double Price{ get; set; }

        [Display(Name = "Fecha", Prompt = "Fecha")]
        public string PublicationDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();

        public int Order { get; set; }

        public Uri FileUrl { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }

    }
}
