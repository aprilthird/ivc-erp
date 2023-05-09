using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.TechnicalOffice
{
   public class UspProcedureProcess
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public Guid? DocumentTypeId { get; set; }

        public string DocumentType { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public Uri FileUrl { get; set; }
        public Uri FileUrl2 { get; set; }


        public string Processes { get; set; }
    }
}
