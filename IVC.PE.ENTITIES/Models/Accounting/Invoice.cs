using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Accounting
{
    public class Invoice
    {
        public Guid Id { get; set; }

        public string Serie { get; set; }

        public DateTime IssueDate { get; set; }

        public Uri FileUrl { get; set; }

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }
    }
}
