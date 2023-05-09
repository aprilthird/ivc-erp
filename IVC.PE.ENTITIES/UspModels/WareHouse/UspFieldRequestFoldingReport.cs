using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.WareHouse
{
    [NotMapped]
    public class UspFieldRequestFoldingReport
    {
        public Guid Id { get; set; }

        public double Quantity { get; set; }
        public double DeliveredQuantity { get; set ; }
        public string Phase { get; set; } 
        public string FullCode { get; set; }
        public string UnitAbbreviation { get; set; }
        public string Description { get; set; }


    }
}
