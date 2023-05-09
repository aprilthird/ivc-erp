using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.General
{
    [NotMapped]
    public class UspWorkFront
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string FormulaCodes { get; set; }
    }
}
