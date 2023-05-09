using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Logistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class ProviderCatalog
    {
        public Guid Id { get; set; }

        public Guid SupplyFamilyId { get; set; }
    public SupplyFamily SupplyFamily { get; set; }

    public Guid SupplyGroupId { get; set; }
    public SupplyGroup SupplyGroup { get; set; }

    public Guid? SpecialityId { get; set; }
    public Speciality Speciality { get; set; }

        public Guid? ProviderId { get; set; }
        public Provider Provider { get; set; }

        public string Name { get; set; }

        public Uri FileUrl { get; set; }
    }
}
