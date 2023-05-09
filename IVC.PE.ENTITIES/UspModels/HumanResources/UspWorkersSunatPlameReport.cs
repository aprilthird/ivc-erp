using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspWorkersSunatPlameReport
    {
        public string Description { get; set; }
        public string Code { get; set; }
        public string SunatCode { get; set; }
        public int CategoryId { get; set; }
        public decimal Value { get; set; }
    }
}
