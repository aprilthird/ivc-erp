using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.PackageViewModels
{
    public class PackageOrderViewModel
    {
        public string ProviderName { get; set; }

        public int Type { get; set; }

        public string Code { get; set; }

        public double Ammount { get; set; }
    }
}
