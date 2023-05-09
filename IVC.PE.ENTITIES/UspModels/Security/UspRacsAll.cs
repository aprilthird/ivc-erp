using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.Security
{
    [NotMapped]
    public class UspRacsAll
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string ProjectAbbr { get; set; }
        public DateTime ReportDate { get; set; }
        public string ReportDateStr => ReportDate.ToDateString();
        public int Status { get; set; }
        public string ReportUser { get; set; }
        public string LiftUser { get; set; }
        public string WorkFrontCode { get; set; }
        public string SewerGroupCode { get; set; }
    }
}
