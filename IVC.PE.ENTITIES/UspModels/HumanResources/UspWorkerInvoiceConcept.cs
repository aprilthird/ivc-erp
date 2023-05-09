using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspWorkerInvoiceConcept
    {
        public string Code { get; set; }
        public string ShortDescription { get; set; }
        public decimal Value { get; set; }
        public string ValueStr => Value.ToString("0.00", CultureInfo.InvariantCulture);
        public int CategoryId { get; set; }
    }
}
