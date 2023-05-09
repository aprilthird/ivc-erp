using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class WorkerWorkPeriod
    {
        public Guid Id { get; set; }

        public Guid? WorkerId { get; set; }
        public Worker Worker { get; set; }

        public DateTime? EntryDate { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public Guid? PensionFundAdministratorId { get; set; }
        public PensionFundAdministrator PensionFundAdministrator { get; set; }
        public string PensionFundUniqueIdentificationCode { get; set; }

        public int Category { get; set; }

        public int Origin { get; set; }

        public int Workgroup { get; set; }

        public Guid? WorkerPositionId { get; set; }
        public WorkPosition WorkerPosition { get; set; }

        public int NumberOfChildren { get; set; }

        public bool HasUnionFee { get; set; }

        public bool HasSctr { get; set; }

        public int SctrHealthType { get; set; }

        public int SctrPensionType { get; set; }

        public decimal JudicialRetentionFixedAmmount { get; set; }

        public decimal JudicialRetentionPercentRate { get; set; }

        public bool HasWeeklySettlement { get; set; }

        public int LaborRegimen { get; set; }

        public bool HasEPS { get; set; }

        public bool HasEsSaludPlusVida { get; set; }

        public DateTime? CeaseDate { get; set; }

        public bool IsActive { get; set; }
    }
}
