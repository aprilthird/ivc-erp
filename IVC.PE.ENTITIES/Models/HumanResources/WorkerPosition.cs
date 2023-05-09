using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class WorkerPosition
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
