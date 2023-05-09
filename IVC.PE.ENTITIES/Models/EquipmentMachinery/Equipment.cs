using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class Equipment
    {
        public Guid Id { get; set; }

        public Guid EquipmentTypeId { get; set; }

        public EquipmentType EquipmentType { get; set; }

        [Required]
        public string SerialNumber { get; set; }

        [Required]
        public string Model { get; set; }

        public string Code { get; set; }

        public string Propietary { get; set; }

        public string Operator { get; set; }

        public int Status { get; set; } = ConstantHelpers.Equipment.Status.OPERATIONAL;
    }
}
