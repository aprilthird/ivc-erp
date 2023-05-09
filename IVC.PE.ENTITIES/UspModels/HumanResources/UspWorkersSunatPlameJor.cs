using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspWorkersSunatPlameJor
    {
        public Guid WorkerId { get; set; }
        public string DocumentType { get; set; }
        public string Document { get; set; }
        public string Code { get; set; }
        public decimal Value { get; set; }
    }
}
