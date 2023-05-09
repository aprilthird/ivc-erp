using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]
    public class UspTransportCostWork
    {

        public Guid EquipmentProviderId { get; set; }

        public string Description { get; set; }

        public string Brand { get; set; }


        public int Year { get; set; }

        public int Month { get; set; }
    
        public double Ammount { get; set; }

        public string AmmountFormatted => String.Format(new CultureInfo("es-PE"), "{0:C}", Ammount);

    }
}
