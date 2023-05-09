using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.Quality
{
    [NotMapped]
    public class UspEquipmentResponsibles
    {
        public Guid ProjectId { get; set; }
        public string ProjectAbbr { get; set; }
        public string UserNames { get; set; }
    }
}
