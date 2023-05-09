using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class PreRequestItem
    {
        public Guid Id { get; set; }

        public Guid PreRequestId { get; set; }
        public PreRequest PreRequest { get; set; }

        public Guid? SupplyId { get; set; }

        public Supply Supply { get; set; }

        public Guid WorkFrontId { get; set; }
        public WorkFront WorkFront { get; set; }

        public double Measure { get; set; }

        public double MeasureInAttention { get; set; }

        public string UsedFor { get; set; }

        public string Observations { get; set; }

        public string SupplyName { get; set; }

        public string MeasurementUnitName { get; set; }
    }
}
