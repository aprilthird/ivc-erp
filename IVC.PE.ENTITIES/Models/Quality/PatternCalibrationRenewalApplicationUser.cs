using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Quality
{
    public class PatternCalibrationRenewalApplicationUser
    {
        public Guid Id { get; set; }
        [Required]
        public Guid PatternCalibrationRenewalId { get; set; }
        public PatternCalibrationRenewal PatternCalibrationRenewal { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
