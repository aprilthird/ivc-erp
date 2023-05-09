using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    public class UspEquipmentMachVal
    {
        public Guid FatherId { get; set; }

        public Guid EquipmentProviderId { get; set; }

        public string TradeName { get; set; }

        public Guid EquipmentMachId { get; set; }

        public string Description { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string Plate { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public int? Week { get; set; }

        public int FoldingNumber { get; set; }

        public double InitHorometer { get; set; }

        public double EndHorometer { get; set; }

        public double UnitPrice { get; set; }

        public string UnitPriceFormatted => String.Format(new CultureInfo("es-PE"), "{0:C}", UnitPrice);

        public string UnitPriceMonthlyFormatted => String.Format(new CultureInfo("es-PE"), "{0:C}", UnitPrice * 30);

        public double AcumulatedHorometer { get; set; }

        public double Ammount { get; set; }
        public string AmmountFormatted => String.Format(new CultureInfo("es-PE"), "{0:C}", Ammount);

        public double Igv { get; set; }
        public string IgvFormatted => String.Format(new CultureInfo("es-PE"), "{0:C}", Igv);

        public double TotalAmmount { get; set; }
        public string TotalAmmountFormatted => String.Format(new CultureInfo("es-PE"), "{0:C}", TotalAmmount);

        public Guid EquipmentMachineryOperatorId { get; set; }

        public int OperatorCount { get; set; }

        public Guid? ProjectPhaseId { get; set; }

        public int PhaseCount { get; set; }
        public Guid SewerGroupid { get; set; }

        public int SewerGroupCount { get; set; }

        public string SewerCode { get; set; }

        public string Code { get; set; }

        public string ActualName { get; set; }

        public string UserName { get; set; }

        public double CountIds { get; set; }

        public string SerieNumber { get; set; }

        public string EquipmentYear { get; set; }
    }
}
