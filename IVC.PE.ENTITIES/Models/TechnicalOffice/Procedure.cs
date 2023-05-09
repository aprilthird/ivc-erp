using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
   public class Procedure
    {
        
        public Guid Id { get; set; }
        public Guid? ProjectId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }


        public Uri FileUrl { get; set; }

        public Uri FileUrl2 { get; set; }
        public Guid? DocumentTypeId { get; set; }

        public DocumentType DocumentType { get; set; }


    }
}
