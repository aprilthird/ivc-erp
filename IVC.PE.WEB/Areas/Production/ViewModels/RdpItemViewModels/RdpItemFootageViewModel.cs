﻿using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Production.ViewModels.RdpItemViewModels
{
    public class RdpItemFootageViewModel
    {
        public Guid? Id { get; set; }

        public Guid RdpItemId { get; set; }
        public RdpItemViewModel RdpItem { get; set; }

        public string RdpDate { get; set; }

        public Guid? SewerGroupId { get; set; }
        public SewerGroupViewModel SewerGroup { get; set; }

        public decimal? PartialAmmount { get; set; }
        public decimal? AccumulatedAmmount { get; set; }
        public decimal? StakeOut { get; set; }
    }
}
