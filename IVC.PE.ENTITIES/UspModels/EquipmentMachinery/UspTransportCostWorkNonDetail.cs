using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]
    public class UspTransportCostWorkNonDetail
    {
        public Guid EquipmentProviderId { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public string TradeName { get; set; }

        public string Code { get; set; }

        public double Ammount { get; set; }

        public string AmmountFormatted => String.Format(new CultureInfo("es-PE"), "{0:C}", Ammount);
        public double Igv { get; set; }

        public string IgvFormatted  => String.Format(new CultureInfo("es-PE"), "{0:C}", Igv);

        public double Total { get; set; }
        public string TotalFormatted => String.Format(new CultureInfo("es-PE"), "{0:C}", Total);
        public Guid TransportPhaseId { get; set; }




    }
}
