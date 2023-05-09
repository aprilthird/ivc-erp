﻿using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeViewModels
{
    public class EquipmentMachineryTypeViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name ="Descripción",Prompt ="Descripción")]
        public string Description { get; set; }
    }
}
