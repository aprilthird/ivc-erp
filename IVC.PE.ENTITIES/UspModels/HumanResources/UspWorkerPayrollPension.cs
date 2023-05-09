using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    public class UspWorkerPayrollPension
    {
        public Guid WorkerId { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public string Code { get; set; }
        public string PensionFundUniqueIdentificationCode { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? CeaseDate { get; set; }
        public decimal Total { get; set; }
    }
}
