﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTypeViewModels
{
    public class BudgetTypeViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }
    }
}
