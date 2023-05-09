﻿using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
   public class InsuranceEntity
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        public string Description { get; set; }
    }
}
