using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspPayrollWorkerVariable
    {
        public Guid Id { get; set; }
        public Guid PayrollVariableId { get; set; }
        public Guid WorkerId { get; set; }
        public string Value { get; set; }
        public string Code { get; set; }
    }
}
