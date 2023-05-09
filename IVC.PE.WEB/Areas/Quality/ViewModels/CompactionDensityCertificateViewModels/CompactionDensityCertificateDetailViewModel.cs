using IVC.PE.WEB.Areas.Quality.ViewModels.FillingLaboratoryTestViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.ViewModels.CompactionDensityCertificateViewModels
{
    public class CompactionDensityCertificateDetailViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Fecha de Ensayo", Prompt = "Fecha de Ensayo")]
        public string TestDate { get; set; }

        [Display(Name = "Muestra N°", Prompt = "Muestra N°")]
        public Guid FillingLaboratoryTestId { get; set; }

        public FillingLaboratoryTestViewModel FillingLaboratoryTest { get; set; }

        [Display(Name = "Densidad Húmeda", Prompt = "Densidad Húmeda")]
        public double WetDensity { get; set; }

        [Display(Name = "% Humedad", Prompt = "% Humedad")]
        public double Moisture { get; set; }

        [Display(Name = "Densidad Seca", Prompt = "Densidad Seca")]
        public double DryDensity { get; set; }

        [Display(Name = "% Densidad", Prompt = "% Densidad")]
        public double DensityPercentage { get; set; }

        [Display(Name = "Capa", Prompt = "Capa")]
        public int Layer { get; set; }
        
        public bool Latest { get; set; }
    }
}
