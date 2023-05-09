using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class EquipmentMachPartFolding
    {
        public Guid Id { get; set; }

        public Guid EquipmentMachPartId { get; set; }

        public EquipmentMachPart EquipmentMachPart { get; set; }

        public string PartNumber { get; set; }
        public DateTime PartDate { get; set; }

        public Guid EquipmentMachineryOperatorId { get; set; }
        public EquipmentMachineryOperator EquipmentMachineryOperator { get; set; }

        public int Order { get; set; }
        public string Specific { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }

        public int? WorkArea { get; set; }

        public double InitHorometer { get; set; }

        public double EndHorometer { get; set; }

        public double Dif { get; set; }

        public Guid EquipmentMachineryTypeTypeActivityId { get; set; }

        public EquipmentMachineryTypeTypeActivity EquipmentMachineryTypeTypeActivity { get; set; }

        public Guid SewerGroupId { get; set; }

        public SewerGroup SewerGroup { get; set; }

        public Guid? MachineryPhaseId     { get; set; }

        public MachineryPhase MachineryPhase { get; set; }

        public Guid? ProjectCalendarWeekId { get; set; }

        public ProjectCalendarWeek ProjectCalendarWeek { get; set; }

    }
}
