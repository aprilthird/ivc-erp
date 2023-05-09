using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class WorkerCovidCheck
    {
        public Guid Id { get; set; }

        public Guid WorkerId { get; set; }

        public Worker Worker { get; set; }

        public string Document { get; set; }

        public DateTime CheckDate { get; set; }

        public int TestType { get; set; }

        public int? IgM { get; set; }

        public int? IgG { get; set; }

        public int? TestOutcome { get; set; }

        public Uri FileUrl { get; set; }
    }
}
