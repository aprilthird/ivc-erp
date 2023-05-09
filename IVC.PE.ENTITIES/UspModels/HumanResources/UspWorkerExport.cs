using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspWorkerExport
    {
        public Guid WorkerId { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string FullName => $"{PaternalSurname} {MaternalSurname} {Name}";
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int DocumentType { get; set; }
        public string Document { get; set; }
        public string DocumentTypeDesc => ConstantHelpers.DocumentType.VALUES[DocumentType];
        public int NumberOfChildren { get; set; }
        public int Gender { get; set; }
        public string GenderDesc => ConstantHelpers.Worker.Category.VALUES[Category];
        public string DocTypeNumber => $"{ConstantHelpers.DocumentType.VALUES[DocumentType]} {Document}";
        public Guid WorkerWorkPeriodId { get; set; }
        public DateTime EntryDate { get; set; }
        public string EntryDateStr => EntryDate.ToDateString();
        public DateTime? CeaseDate { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthDateStr => BirthDate.HasValue ? BirthDate.Value.ToDateString() : string.Empty;
        public string CeaseDateStr => CeaseDate.HasValue ? CeaseDate.Value.ToDateString() : string.Empty;
        public int Category { get; set; }
        public string CategoryDesc => ConstantHelpers.Worker.Gender.VALUES[Gender];
        public decimal CategoryWage { get; set; }
        public int Origin { get; set; }
        public string OriginDesc => ConstantHelpers.Worker.Origin.VALUES[Origin];
        public int Workgroup { get; set; }
        public string WorkgroupDesc => ConstantHelpers.Worker.Workgroup.VALUES[Workgroup];
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string PensionFundCode { get; set; }
        public string PensionFundUniqueIdentificationCode { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveStr => ConstantHelpers.EntityStatus.VALUES[IsActive];
        public string WorkPositionName { get; set; }
    }
}
