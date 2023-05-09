using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspWorkerPayrollPensionReport
    {
        public string Document { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string PensionCode { get; set; }
        public string PensionCussp { get; set; }
        public string Code { get; set; }
        public string ShortDescription { get; set; }
        public decimal Value { get; set; }
    }
}
