using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]
    public class UspEquipmentProvider
    {
        public Guid Id { get; set; }

        public Guid ProviderId { get; set; }

        public string Tradename { get; set; }

        public string RUC { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string CollectionAreaContactName { get; set; }

        public string CollectionAreaPhoneNumber { get; set; }

        public string CollectionAreaEmail { get; set; }

        public string Equips { get; set; }

        public Guid? FoldingId { get; set; }

        public Guid? EquipmentMachineryTypeId { get; set; }

        public Guid? EquipmentMachineryTypeTypeId { get; set; }

        public Guid? EquipmentMachineryTypeTransportId { get; set; }

        public Guid? EquipmentMachineryTypeSoftId { get; set; }

    }
}
