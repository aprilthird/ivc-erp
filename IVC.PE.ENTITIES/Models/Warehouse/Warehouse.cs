using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Warehouse
{
    public class Warehouse
    {
        public Guid Id { get; set; }

        public Guid WarehouseTypeId { get; set; }
        public WarehouseType WarehouseType { get; set; }

        public Guid WorkFrontId { get; set; }
        public WorkFront WorkFront { get; set; }

        public string Address { get; set; }
        public Uri GoogleMapsUrl { get; set; }
    }
}
