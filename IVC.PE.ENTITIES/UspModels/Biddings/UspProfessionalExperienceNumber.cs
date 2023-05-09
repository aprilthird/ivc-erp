using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.Biddings
{
    [NotMapped]
    public class UspProfessionalExperienceNumber
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string PaternalSurname { get; set; }

        public string MaternalSurname { get; set; }

        public int Dif{ get; set; }

        public int Years { get; set; }

        public int Months { get; set; }

        public int Days { get; set; }


    }
}
