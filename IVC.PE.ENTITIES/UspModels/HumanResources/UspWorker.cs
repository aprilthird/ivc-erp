using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspWorker
    {
        public Guid WorkerId { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string FullName => $"{PaternalSurname} {MaternalSurname} {Name}";
        public int DocumentType { get; set; }
        public string Document { get; set; }
        public string DocTypeNumber => $"{ConstantHelpers.DocumentType.VALUES[DocumentType]} {Document}";
        public Guid WorkerWorkPeriodId { get; set; }
        public DateTime EntryDate { get; set; }
        public string EntryDateStr => EntryDate.ToDateString();
        public DateTime? CeaseDate { get; set; }
        public string CeaseDateStr => CeaseDate.HasValue ? CeaseDate.Value.ToDateString() : string.Empty;
        public int Category { get; set; }
        public string CategoryDesc => ConstantHelpers.Worker.Category.VALUES[Category];
        public decimal CategoryWage { get; set; }
        public int Origin { get; set; }
        public string OriginDesc => ConstantHelpers.Worker.Origin.VALUES[Origin];
        public int Workgroup { get; set; }
        public string WorkgroupDesc => ConstantHelpers.Worker.Workgroup.VALUES[Workgroup];
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime? EmailConfirmationDateTime { get; set; }
        public string EmailConfirmationDateTimeStr => EmailConfirmationDateTime.HasValue ? EmailConfirmationDateTime.Value.ToDateString() : string.Empty;
        public DateTime? EmailAlertSentDateTime { get; set; }
        public string EmailAlertSentDateTimeStr => EmailAlertSentDateTime.HasValue ? EmailAlertSentDateTime.Value.ToDateString() : string.Empty;
        public string PensionFundCode { get; set; }
        public bool IsActive { get; set; }
        public string WorkPositionName { get; set; }
    }
}
