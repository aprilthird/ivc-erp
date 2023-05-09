using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.Models.DocumentaryControl
{
    public class WorkbookSeat
    {
        public Guid Id { get; set; }
        
        public int Number { get; set; }

        public Guid WorkbookId { get; set; }

        public Workbook Workbook { get; set; }

        public int WroteBy { get; set; }

        public DateTime Date { get; set; }

        public DateTime? ResponseDate { get; set; }

        public int Status { get; set; }

        public string Subject { get; set; }

        public int Type { get; set; }

        public Guid? WorkbookTypeId { get; set; }
        public WorkbookType WorkbookType { get; set; }
        public bool Answered { get; set; }

        public Uri FileUrl { get; set; }

        public string Detail { get; set; }
    }
}
