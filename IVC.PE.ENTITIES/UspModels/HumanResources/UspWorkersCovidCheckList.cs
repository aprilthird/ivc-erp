using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspWorkersCovidCheckList
    {
        public string Document { get; set; }
        public DateTime CheckDate { get; set; }
        public int TestType { get; set; }
        public int? IgM { get; set; }
        public int? IgG { get; set; }
        public int? TestOutcome { get; set; }
        public Uri FileUrl { get; set; }
    }
}
