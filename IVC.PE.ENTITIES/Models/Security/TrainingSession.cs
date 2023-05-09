using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Security
{
    public class TrainingSession
    {
        public Guid Id { get; set; }

        public DateTime SessionDate { get; set; }
        
        public TrainingTopic TrainingTopic { get; set; }

        public Guid TrainingTopicId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public string UserId { get; set; }

        public WorkFront WorkFront { get; set; }

        public Guid WorkFrontId { get; set; }

        public IEnumerable<TrainingSessionWorkerEmployee> TrainingSessionWorkerEmployees { get; set; }
    }
}
