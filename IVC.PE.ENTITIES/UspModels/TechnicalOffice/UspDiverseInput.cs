using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.TechnicalOffice
{
    [NotMapped]
    public class UspDiverseInput
    {
        public Guid Id { get; set; }

        public string WorkFrontCode { get; set; }

        public string ProjectPhaseName { get; set; }

        public string SupplyFamilyName { get; set; }

        public string SupplyGroupName { get; set; }

        public string ItemNumber { get; set; }

        public string Description { get; set; }

        public string SupplyFullCode { get; set; }

        public string SupplyDescription { get; set; }

        public string MeasurementUnitAbbreviation { get; set; }

        public double Metered { get; set; }

        public string MeteredString => Metered.ToString("N2", CultureInfo.InvariantCulture);

        public double UnitPrice { get; set; }

        public string UnitPriceString => UnitPrice.ToString("N2", CultureInfo.InvariantCulture);

        public double Parcial { get; set; }

        public string ParcialString => Parcial.ToString("N2", CultureInfo.InvariantCulture);

        public string BudgetInputCode { get; set; }
    }
}
