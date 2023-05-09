using DocumentFormat.OpenXml.Office.CoverPageProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.PackageViewModels
{
    public class PackageGroupViewModel
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public double SaleAmmount { get; set; }

        public double GoalAmmount { get; set; }
    }
}
