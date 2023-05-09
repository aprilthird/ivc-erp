using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice
{
    public class TechnicalLibraryResourceModel
    {
        public Guid Id { get; set; }

        public Guid? SpecialityId { get; set; }

        public string SpecDescription { get; set; }

        public string Name { get; set; }
        
        public string Author { get; set; }

        public string TechLibraryDate { get; set; }

        public Uri FileUrl { get; set; }


    }
}
