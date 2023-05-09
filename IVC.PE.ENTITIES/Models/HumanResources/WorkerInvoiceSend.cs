using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class WorkerInvoiceSend
    {
        public Guid Id { get; set; }

        public Guid PayrollMovementHeaderId { get; set; }
        public PayrollMovementHeader PayrollMovementHeader { get; set; }

        public Guid WorkerId { get; set; }
        public Worker Worker { get; set; }

        public DateTime? DateSended { get; set; }

        public string Observation { get; set; }
    }
}
