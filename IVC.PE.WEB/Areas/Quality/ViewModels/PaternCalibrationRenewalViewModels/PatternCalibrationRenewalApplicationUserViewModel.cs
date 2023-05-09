using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.ViewModels.PaternCalibrationRenewalViewModels
{
    public class PatternCalibrationRenewalApplicationUserViewModel
    {
        public Guid? Id { get; set; }
        public Guid? PatternCalibrationRenewalId { get; set; }
        public PatternCalibrationRenewalViewModel PatternCalibrationRenewal { get; set; }
        public String UserId { get; set; }
    }
}
