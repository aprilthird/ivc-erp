using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]
    public class UspMachCostWork
    {

        public Guid EquipmentProviderId { get; set; }

        public Guid EquipmentMachineryTypeTypeId { get; set; }

        public string Description { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public double? Counts { get; set; }

        public double? Up { get; set; }

        public Guid SewerGroupId { get; set; }
        public Guid MachineryPhaseId { get; set; }
        public string AmmountFormatted => String.Format(new CultureInfo("es-PE"), "{0:C}", Up.Value);
    }
}
