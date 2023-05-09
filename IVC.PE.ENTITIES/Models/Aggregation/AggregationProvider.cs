using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Aggregation
{
    public class AggregationProvider
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LicensePlate { get; set; }
        public string Volume { get; set; }
        public Uri VolumeCertificate { get; set; }
    }
}
