using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]
   public class UspEquipmentTransportVal
    {
        public Guid EquipmentMachineryTransportPartId { get; set; }

        public Guid EquipmentProviderId { get; set; }

        public string TradeName { get; set; }

        public Guid EquipmentMachineryTransportId { get; set; }

        public string Description { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string EquipmentPlate { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public double LastInitMileage { get; set; }

        public double LastEndMileage { get; set; }

        public double AcumulatedMileage { get; set; }

        public int FoldingNumber { get; set; }

        public double UnitPrice { get; set; }
        public string UnitPriceFormatted => String.Format(new CultureInfo("es-PE"), "{0:C}", UnitPrice);

        public string UnitPriceMonthlyFormatted => String.Format(new CultureInfo("es-PE"), "{0:C}", UnitPrice*30);



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

        public string Code { get; set; }

        public int HiringType { get; set; }

        public string OperatorName { get; set; }

        public string FromOtherName { get; set; }

        public string WorkerName { get; set; }

        public string WorkerMiddleName { get; set; }

        public string WorkerPaternalSurName { get; set; }

        public string WorkerMaternalSurName { get; set; }

        public string UserName { get; set; }

        public Guid FatherId { get; set; }
    }
}
