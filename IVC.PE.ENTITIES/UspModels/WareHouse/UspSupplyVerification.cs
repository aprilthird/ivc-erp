using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.WareHouse
{
    [NotMapped]
    public class UspSupplyVerification
    {
        public string Description { get; set; }

        public string CorrelativeCodeStr { get; set; }

        public string IvcCode { get; set; }

        public string Tradename { get; set; }



        public DateTime DeliveryDate { get; set; }

        public string RemissionGuide { get; set; }

        public double Measure { get; set; }
        public Guid SupplyId { get; set; }
    }
}
