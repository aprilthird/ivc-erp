using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.WareHouse
{
    [NotMapped]
    public class UspFieldRequestReport
    {

        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }
        public int DocumentNumber { get; set; }
        public string Observation { get; set; } 
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string WorkFrontCode { get; set; }
        public string SewerGroupCode { get; set; }
        public string FormulaCodes { get; set; }

        public string WorkOrder { get; set; }
    }

}
