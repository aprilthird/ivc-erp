﻿using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Logistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class FuelProvider
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public Guid ProviderId { get; set; }

        public Provider Provider {get; set;}

        public double LastPrice { get; set; }

        public int PriceNumber { get; set; }

    }
}
