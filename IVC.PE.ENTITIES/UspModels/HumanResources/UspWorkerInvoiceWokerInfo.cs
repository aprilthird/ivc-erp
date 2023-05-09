using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspWorkerInvoiceWokerInfo
    {
        public string CompanyName { get; set; }
        public string CompanyRuc { get; set; }
        public string ProjectName { get; set; }
        public Uri ProjectLogoUrl { get; set; }
        public Uri ProjectInvoiceSignatureUrl { get; set; }
        public string Name { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public int Category { get; set; }
        public string CategoryStr => ConstantHelpers.Worker.Category.VALUES[Category];
        public string CategoryBucStr => ConstantHelpers.Worker.Category.BUC_VALUES[Category];
        public DateTime EntryDate { get; set; }
        public string EntryDateStr => EntryDate.ToDateString();
        public string Document { get; set; }
        public DateTime? CeaseDate { get; set; }
        public string CeaseDateStr => CeaseDate.HasValue ? CeaseDate.Value.ToDateString() : string.Empty;
        public string PensionName { get; set; }
        public string PensionCussp { get; set; }
    }
}
