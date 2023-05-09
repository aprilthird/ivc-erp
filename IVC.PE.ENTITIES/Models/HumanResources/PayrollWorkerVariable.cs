using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class PayrollWorkerVariable
    {
        public Guid Id { get; set; }

        public Guid WorkerId { get; set; }

        public Worker Worker { get; set; }

        public Guid PayrollVariableId { get; set; }

        public PayrollVariable PayrollVariable { get; set; }

        public Guid PayrollMovementHeaderId { get; set; }

        public PayrollMovementHeader PayrollMovementHeader { get; set; }

        public string Value { get; set; }
    }
}
