using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice
{
    public class BimResourceModel
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public Guid ProjectFormulaId { get; set; }

        public string ProjectFormulaCode { get; set; }

        public Guid WorkFrontId { get; set; }

        public string WorkFrontCode { get; set; }

        public Uri FileUrl { get; set; }

        public string Name { get; set; }

    }
}
