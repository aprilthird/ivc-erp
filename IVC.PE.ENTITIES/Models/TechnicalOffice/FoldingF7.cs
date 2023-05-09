using IVC.PE.ENTITIES.Models.Production;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class FoldingF7
    {
        public Guid Id { get; set; }
        public Guid ProductionDailyPartId { get; set; }
        public ProductionDailyPart ProductionDailyPart { get; set; }
        public DateTime Date { get; set; }
        public double ExcavatedLength { get; set; }
        public double InstalledLength { get; set; }
        public double RefilledLength { get; set; }
        public double GranularBaseLength { get; set; }

    }
}
