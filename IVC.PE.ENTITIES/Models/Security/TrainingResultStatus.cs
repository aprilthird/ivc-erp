using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Security
{
    public class TrainingResultStatus
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Color { get; set; } = ConstantHelpers.Training.ResultStatusColor.GRAY;
    }
}