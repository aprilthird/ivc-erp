
using IVC.PE.ENTITIES.Models.Quality;
using IVC.PE.WEB.Areas.Quality.ViewModels.FillingLaboratoryTestViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.SewerManifoldFor05ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.ViewModels.FoldingFor05ViewModels
{
    public class FoldingFor05ViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Número Capa", Prompt = "Número Capa")]
        public string LayerNumber { get; set; }
        [Display(Name = "Fecha de Ensayo", Prompt = "Fecha Ensayo")]
        public string TestDate { get; set; }
        public Guid SewerManifoldFor05Id { get; set; }
        public SewerManifoldFor05ViewModel sewerManifoldFor05 { get; set; }
        public Guid FillingLaboratoryTestId { get; set; }
        public FillingLaboratoryTestViewModel FillingLaboratoryTest { get; set; }

        [Display(Name = "Densidad Humeda", Prompt = "Densidad Humeda")]
        public string WetDensity { get; set; }
        [Display(Name = "Porcentaje Humedad", Prompt = "Porcentaje Humedad")]
        public string MoisturePercentage { get; set; }
        [Display(Name = "Densidad Seca", Prompt = "Densidad Seca")]
        public string DryDensity { get; set; }
        [Display(Name = "Porcentaje Compactación Requerido", Prompt = "Porcentaje Compactación Requerido")]
        public string PercentageRequiredCompaction { get; set; }
        [Display(Name = "Porcentaje Compactación Real", Prompt = "Porcentaje Compactación Real")]
        public string PercentageRealCompaction { get; set; }
        [Display(Name = "Estado", Prompt = "Estado")]
        public string Status { get; set; }
    }
}

