using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspPayrollInvoiceDetail
    {
        public Guid WorkerId { get; set; }
        public string Code { get; set; }
        public string ShortDescription { get; set; }
        public decimal Value { get; set; }
    }
}
