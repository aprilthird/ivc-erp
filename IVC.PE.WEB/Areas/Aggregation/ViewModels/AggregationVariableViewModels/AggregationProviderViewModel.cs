using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Aggregation.ViewModels.AggregationVariableViewModels
{
    public class AggregationProviderViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string LicensePlate { get; set; }
        public string Volume { get; set; }
        public Uri VolumeCertificate { get; set; }
    }
}
