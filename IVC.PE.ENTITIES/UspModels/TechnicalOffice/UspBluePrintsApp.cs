using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.TechnicalOffice
{
    [NotMapped]
    public class UspBluePrintsApp
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public Uri FileUrl { get; set; }

        public Uri LetterUrl { get; set; }
    }
}
