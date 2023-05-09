using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspWorkerPayrollDetail
    {
        public Guid Id { get; set; }
        public Guid WorkerId { get; set; }
        public string ConceptCode { get; set; }
        public decimal ConceptValue { get; set; }
    }
}
