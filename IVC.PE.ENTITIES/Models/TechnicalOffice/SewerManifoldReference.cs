using IVC.PE.ENTITIES.Models.Production;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class SewerManifoldReference
    {
        public Guid Id { get; set; }

        public Guid SewerManifoldReviewId { get; set; }
        public SewerManifold SewerManifoldReview { get; set; }
        public Guid? ProductionDailyPartId { get; set; }
        public ProductionDailyPart ProductionDailyPart { get; set; }

        public Guid SewerManifoldExecutionId { get; set; }
        public SewerManifold SewerManifoldExecution { get; set; }
    }
}
