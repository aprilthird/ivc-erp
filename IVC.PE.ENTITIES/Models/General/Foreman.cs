using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.Models.General
{
    public class Foreman
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PaternalSurname { get; set; }

        [Required]
        public string MaternalSurname { get; set; }

        [NotMapped]
        public string FullName => $"{Name}, {PaternalSurname} {MaternalSurname}";

        public DateTime? BirthDate { get; set; }

        public int DocumentType { get; set; } = ConstantHelpers.DocumentType.ID_CARD;

        [Required]
        public string Document { get; set; }

        public string Role { get; set; }
    }
}
