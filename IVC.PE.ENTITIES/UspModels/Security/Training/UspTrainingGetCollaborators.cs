using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.Security.Training
{
    [NotMapped]
    public class UspTrainingGetCollaborators
    {
        public Guid Id { get; set; }
        public string Document { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsActive { get; set; }
    }
}
