using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.TechnicalOffice
{
    public class UspTechnicalSpec
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ProjectId { get; set; }
        public Guid SupplyFamilyId { get; set; }

        public string SupplyFamilyCode { get; set; }
        public string SupplyFamilyName { get; set; }

        public Guid SupplyGroupId { get; set; }

        public string SupplyGroupCode { get; set; }
        public string SupplyGroupName { get; set; }

        public Uri FileUrl { get; set; }

        public string Specialities { get; set; }
    }
}
