using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Warehouse.ViewModels.ValuedSupplyEntryViewModels
{
    public class ValuedSupplyEntryViewModel
    {
        public string Groups { get; set; }

        public Guid ProviderId { get; set; }

        public string Provider { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

        public double Parcial { get; set; }

        public double DolarParcial { get; set; }

        public string ParcialString { get; set; }

        public string DolarParcialString { get; set; }
    }
}
