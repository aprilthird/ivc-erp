using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class WorkerCategory
    {
        public Guid Id { get; set; }

        public int WorkerCategoryId { get; set; }

        public decimal DayWage { get; set; }

        public decimal BUCRate { get; set; }
    }
}
