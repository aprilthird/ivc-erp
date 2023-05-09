using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class PayrollWorkerCategoryWage
    {
        public Guid Id { get; set; }

        [Required]
        public Guid PayrollMovementHeaderId { get; set; }

        public PayrollMovementHeader PayrollMovementHeader { get; set; }

        [Required]
        public int WorkerCategoryId { get; set; }

        public decimal DayWage { get; set; }

        public decimal BUCRate { get; set; }
    }
}
