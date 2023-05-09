using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.IntegratedManagementSystem
{
    public class For24SecondPartEquipment
    {
        public Guid Id { get; set; }
        public Guid SewerManifoldFor24SecondPartId { get; set; }
        public SewerManifoldFor24SecondPart SewerManifoldFor24SecondPart { get; set; }
        public string EquipmentName { get; set; }
        public int EquipmentQuantity { get; set; }
        public double EquipmentHours { get; set; }
        public double EquipmentTotalHours { get; set; }
    }
}
