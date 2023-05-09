using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspWorkerImport
    {
        public Guid WorkerId { get; set; }
        public string Document { get; set; }
        public Guid PeriodId { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime? CeaseDate { get; set; }
        public bool IsActive { get; set; }
    }
}
