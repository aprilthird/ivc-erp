using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice
{
   public class ProviderCatalogResourceModel
    {
        public Guid Id { get; set; }

        public Guid SupplyFamilyId { get; set; }
        public Guid SupplyGroupId { get; set; }
        public Guid SpecialityId { get; set; }

        public Guid? ProviderId { get; set; }
        public string FamilyDescription { get; set; }

        public string GroupDescription { get; set; }

        public string SpecDescription { get; set; }

        public Uri FileUrl { get; set; }
        public string Name { get; set; }

        public string ProviderDescription { get; set; }

    }
}
