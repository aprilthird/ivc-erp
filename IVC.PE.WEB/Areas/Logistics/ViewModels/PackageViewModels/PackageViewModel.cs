using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.PackageViewModels
{
    public class PackageViewModel             
    {
        public Guid Id { get; set; }

        public string Item { get; set; }

        public string Insumos { get; set; }

        public bool IsFamily { get; set; }

        public bool IsGroup { get; set; }

        public string SaleSupply { get; set; }

        public string GeneratedOrderSupply { get; set; }

        public string AddedSupply { get; set; }

        public string VirtualOutputSupply { get; set; }

        public string UsedSupply { get; set; }

        public string PaidSupply { get; set; }

        public string ValuedSupply { get; set; }

    }
}
