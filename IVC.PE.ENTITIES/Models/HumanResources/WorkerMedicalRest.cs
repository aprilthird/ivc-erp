using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class WorkerMedicalRest
    {
        public Guid Id { get; set; }
        public Guid WorkerId { get; set; }
        public Worker Worker { get; set; }
        public int FileType { get; set; }
        public Uri FileUrl { get; set; }
        public DateTime InitDate { get; set; }
        public int DurationDays { get; set; }
        public DateTime EndDate { get; set; }
    }
}
