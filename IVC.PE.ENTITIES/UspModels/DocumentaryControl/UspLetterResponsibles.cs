using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.DocumentaryControl
{
    [NotMapped]
    public class UspLetterResponsibles
    {
        public Guid ProjectId { get; set; }
        public string ProjectAbbr { get; set; }
        public string UserNames { get; set; }

        public string UserIds { get; set; }
    }
}
