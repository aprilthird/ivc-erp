using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class FuelProviderPriceFolding
    {
        public Guid Id { get; set; }

        public Guid FuelProviderId { get; set; }

        public FuelProvider FuelProvider { get; set; }

        public DateTime PublicationDate { get; set; }

        public double Price { get; set; }

        public int Order { get; set; }

        public Uri FileUrl { get; set; }
    }
}
