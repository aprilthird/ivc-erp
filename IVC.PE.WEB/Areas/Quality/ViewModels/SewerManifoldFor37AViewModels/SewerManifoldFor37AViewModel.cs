
namespace IVC.PE.WEB.Areas.Quality.ViewModels.SewerManifoldFor37AViewModels
{
    using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
    using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerManifoldViewModels;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class SewerManifoldFor37AViewModel
    {
        public Guid? Id { get; set; }
        public Guid ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }
        [Display(Name = "Tramos Ejecución", Prompt = "Tramos Ejecución")]
        public Guid SewerManifoldId { get; set; }
        public SewerManifoldViewModel SewerManifold { get; set; }
        public string For01ProtocolNumber { get; set; }
        public string HotMeltsNumber { get; set; }
        public string ElectrofusionsNumber { get; set; }
        public string ElectrofusionsPasNumber { get; set; }
        public string StartElectrofusionDate { get; set; }
        public string EndElectrofusionDate { get; set; }
        [Display(Name = "Lote de Tuberias 1", Prompt = "Lote de Tuberias 1")]
        public string FirstPipeBatch { get; set; }
        [Display(Name = "Lote de Tuberias 2", Prompt = "Lote de Tuberias 2")]
        public string SecondPipeBatch { get; set; }
        [Display(Name = "Lote de Tuberias 3", Prompt = "Lote de Tuberias 3")]
        public string ThridPipeBatch { get; set; }
        [Display(Name = "Lote de Tuberias 4", Prompt = "Lote de Tuberias 4")]
        public string ForthPipeBatch { get; set; }
        public Uri FileUrl { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }
    }
}
