using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice
{
    public class ProcedureResourceModel
    {
        public Guid Id { get; set; }

        public Guid? ProjectId { get; set; }
        public string Processes { get; set; }

        public Guid? DocumentTypeId { get; set; }

        public string DocumentType { get; set; }
        public string Name { get; set; }

        public Uri FileUrl { get; set; }

    }
}
