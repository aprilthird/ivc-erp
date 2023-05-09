using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice
{
   public class TechnicalSpecResourceModel
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public Guid SupplyFamilyId { get; set; }

        public Guid SupplyGroupId { get; set; }

        public string FamilyCode { get; set; }

        public string GroupCode { get; set; }

        public string SpecDescription { get; set; }

        public string Name { get; set; }

        public Uri FileUrl { get; set; }

    }
}
