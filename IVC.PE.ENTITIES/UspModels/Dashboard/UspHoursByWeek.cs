using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.Dashboard
{
    [NotMapped]
    public class UspHoursByWeek
    {
        public string YearWeekNumber { get; set; }
        public decimal? Hours { get; set; }
    }
}
