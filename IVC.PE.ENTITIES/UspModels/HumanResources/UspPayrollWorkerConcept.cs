using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspPayrollWorkerConcept
    {
        public Guid WorkerId { get; set; }
        public Guid PayrollConceptId { get; set; }
        public string Code { get; set; }
        public decimal Value { get; set; }
    }
}
