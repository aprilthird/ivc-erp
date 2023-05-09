﻿using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels
{
    public class MeasurementUnitViewModel
    {
        [Display(Name = "Id")]
        public Guid? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Abreviación", Prompt = "Abreviación")]
        public string Abbreviation { get; set; }

        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }

        public IList<MeasurementUnitViewModel> ChildrenModels { get; set; }
    }
}
