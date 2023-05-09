using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspPayrollPreviousTaxes
    {
        public string Code { get; set; }
        public string Value { get; set; }
        public int Year { get; set; }
        public int WeekNumber { get; set; }
        public Guid WorkerId { get; set; }
    }
}
