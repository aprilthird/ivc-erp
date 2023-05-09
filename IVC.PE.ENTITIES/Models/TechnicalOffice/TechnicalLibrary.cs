using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class TechnicalLibrary
    {
        public Guid Id { get; set; }


        public Guid? SpecialityId { get; set; }

        public Speciality Speciality { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public DateTime LibraryDate { get; set; }

        public Uri FileUrl { get; set; }
    }
}
