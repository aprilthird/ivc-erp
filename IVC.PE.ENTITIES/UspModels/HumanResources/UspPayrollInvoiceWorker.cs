using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspPayrollInvoiceWorker
    {
        public Guid WorkerId { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string FullName => $"{PaternalSurname} {MaternalSurname} {Name}";
        public DateTime EntryDate { get; set; }
        public string EntryDateSrt => EntryDate.ToDateString();
        public DateTime? CeaseDate { get; set; }
        public string CeaseDateStr => CeaseDate.HasValue ? CeaseDate.Value.ToDateString() : string.Empty;
        public string PensionFund { get; set; }
        public string PensionFundUniqueIdentificationCode { get; set; }
        public string Document { get; set; }
        public int Category { get; set; }
        public string SewerGroupCode { get; set; }
        public string Email { get; set; }
    }
}
