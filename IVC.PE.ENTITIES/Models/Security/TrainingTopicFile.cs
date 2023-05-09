using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Security
{
    public class TrainingTopicFile
    {
        public Guid Id { get; set; }
        public TrainingTopic TrainingTopic { get; set; }
        public Guid TrainingTopicId { get; set; }
        public Uri Url { get; set; }
    }
}
