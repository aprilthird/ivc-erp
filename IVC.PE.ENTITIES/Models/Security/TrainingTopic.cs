using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Security
{
    public class TrainingTopic
    {
        public Guid Id { get; set; }
        public TrainingCategory TrainingCategory { get; set; }
        public Guid TrainingCategoryId { get; set; }
        public Project Project { get; set; }
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
    }
}