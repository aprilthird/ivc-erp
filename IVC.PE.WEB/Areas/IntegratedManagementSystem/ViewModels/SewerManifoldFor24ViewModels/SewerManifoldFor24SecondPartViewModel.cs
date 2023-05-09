using IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.For24ExtrasViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.SewerManifoldFor24ViewModels
{
    public class SewerManifoldFor24SecondPartViewModel
    {
        public Guid? Id { get; set; }
        public Guid SewerManifoldFor24FirstPartId { get; set; }
        public SewerManifoldFor24FirstPartViewModel SewerManifoldFor24FirstPart { get; set; }
        public string ProposedDate { get; set; }
        //------------------------Decisiones sobre el P/S No Conforme
        [Display(Name = "Opciones")]
        public int Decision { get; set; }
        [Display(Name = "Especificar", Prompt = "Especificar")]
        public string Other { get; set; }
        //------------------------Acciones Tomadas

        //------------------------Cuantificación de horas de retrabajos
        [Display(Name = "Cantidad", Prompt = "Cantidad")]
        public int LaborerQuantity { get; set; }
        [Display(Name = "Horas Hombre", Prompt = "Horas Hombre")]
        public string LaborerHoursMan { get; set; }
        [Display(Name = "Total Horas", Prompt = "Total Horas")]
        public string LaborerTotalHoursMan { get; set; }
        [Display(Name = "Cantidad", Prompt = "Cantidad")]
        public int OfficialQuantity { get; set; }
        [Display(Name = "Horas Hombre", Prompt = "Horas Hombre")]
        public string OfficialHoursMan { get; set; }
        [Display(Name = "Total Horas", Prompt = "Total Horas")]
        public string OfficialTotalHoursMan { get; set; }
        [Display(Name = "Cantidad", Prompt = "Cantidad")]
        public int OperatorQuantity { get; set; }
        [Display(Name = "Horas Hombre", Prompt = "Horas Hombre")]
        public string OperatorHoursMan { get; set; }
        [Display(Name = "Total Horas", Prompt = "Total Horas")]
        public string OperatorTotalHoursMan { get; set; }
        //------------------------Regristro fotográfico
        public Uri FileUrl { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }
        public Uri VideoUrl { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Video", Prompt = "Video")]
        public IFormFile Video { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Fotos", Prompt = "Fotos")]
        [JsonIgnore]
        public IFormFileCollection GalleryFiles { get; set; }
        public List<For24SecondPartGalleryViewModel> Gallery { get; set; }
    }
}
