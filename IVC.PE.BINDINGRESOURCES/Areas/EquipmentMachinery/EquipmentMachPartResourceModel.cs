using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.BINDINGRESOURCES.Areas.EquipmentMachinery
{
    public class EquipmentMachPartResourceModel
    {

        public Guid Id { get; set; }

        public Guid EquipmentProviderId { get; set; }

        public Guid EquipmentMachId { get; set; }

        public Guid EquipmentMachineryTypeTypeId { get; set; }

        public Guid ProjectId { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }


        public int FoldingNumber { get; set; }
        //
    }

    public class EquipmentMachPartConsultingResourceModel
    {
        public Guid EquipmentMachId { get; set; }

        public Guid EquipmentMachineryTypeTypeId { get; set; }
    }

    public class EquipmentMachineryOperatorResourceModel
    {
        public Guid Id { get; set; }

        public string ActualName { get; set; }
    }

    public class EquipmentMachineryVerificationResourceModel
    {
        public string Year { get; set; }

        public string Model { get; set; }

        public string Provider { get; set; }

        public string Equipment { get; set; }
    }

    public class EquipmentMachPartFoldingResourceModel
    {

        public Guid Id { get; set; }

        public Guid EquipmentMachPartId { get; set; }

        public string PartNumber { get; set; }
        public DateTime PartDate { get; set; }

        public Guid EquipmentMachineryOperatorId { get; set; }

        public int Order { get; set; }

        public string Specific { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }

        public int? WorkArea { get; set; }

        public double InitHorometer { get; set; }

        public double EndHorometer { get; set; }

        public double Dif { get; set; }

        public Guid EquipmentMachineryTypeTypeActivityId { get; set; }

        public Guid SewerGroupId { get; set; }

        public Guid? MachineryPhaseId { get; set; }
    }
}
