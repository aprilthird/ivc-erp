using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class EquipmentMachinerySoftInsuranceFolding
    {
        public Guid Id { get; set; }

        public Guid EquipmentMachinerySoftId { get; set; }


        public EquipmentMachinerySoft EquipmentMachinerySoft { get; set; }

        public DateTime? StartDateInsurance { get; set; }

        public DateTime? EndDateInsurance { get; set; }

        public Uri InsuranceFileUrl { get; set; }

        public int OrderInsurance { get; set; }

        public string Number { get; set; }

        public bool Days30 { get; set; } = false;

        public bool Days15 { get; set; } = false;

        public Guid? InsuranceEntityId { get; set; }

        public InsuranceEntity InsuranceEntity { get; set; }
    }
}
