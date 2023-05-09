using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.PaternCalibrationRenewalViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.ViewModels.PatternCalibrationViewModels
{
    public class PatternCalibrationViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }
        public Guid ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }

        public PatternCalibrationRenewalViewModel PatternCalibrationRenewal { get; set; }
        public IEnumerable<PatternCalibrationRenewalViewModel> PatternCalibrationRenewals { get; set; }

        public int NumberOfRenovations { get; set; }
    }
}
