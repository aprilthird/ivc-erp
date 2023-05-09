using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class TechnicalSpecSpeciality
    {

        public Guid Id { get; set; }

        public Guid TechnicalSpecId { get; set; }

        public TechnicalSpec TechnicalSpec { get; set; }

        public Guid SpecialityId { get; set; }

        public Speciality Speciality { get; set; }
    }
}
