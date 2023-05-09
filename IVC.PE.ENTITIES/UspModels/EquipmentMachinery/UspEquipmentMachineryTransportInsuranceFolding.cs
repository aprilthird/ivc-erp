using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]

    public class UspEquipmentMachineryTransportInsuranceFolding
    {
        public string BrandSPI { get; set; }

        public string ModelSPI { get; set; }

        public string EquipmentPlateSPI { get; set; }

        public int OrderInsurance { get; set; }

        public int ValidityIValiditySPI { get; set; }

        public DateTime? StartDateInsurance { get; set; }

        public DateTime? EndDateInsurance { get; set; }

        public Uri InsuranceFileUrl { get; set; }

        public string TradeNameSPI { get; set; }
    }
}
