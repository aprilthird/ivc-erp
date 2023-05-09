using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]
    public class UspFuelMachValDetail
    {
        public DateTime PartDate { get; set; }
        public string PartDateString => PartDate.ToDateString();

        public double Gallon { get; set; }

        public double Total { get; set; }

        public double Price { get; set; }

        public string TotalFormatted => String.Format(new CultureInfo("es-PE"), "{0:C}", Total);

    }
}
