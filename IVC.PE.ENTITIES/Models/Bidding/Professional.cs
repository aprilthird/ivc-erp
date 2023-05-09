using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Bidding
{
    public class Professional
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string PaternalSurname { get; set; }

        [Required]
        public string MaternalSurname { get; set; }

        [Required]
        public string Document { get; set; }

        [NotMapped]
        public string FullName => $"{PaternalSurname} {MaternalSurname}, {Name}{(string.IsNullOrEmpty(MiddleName) ? string.Empty : $" {MiddleName}")}";

        [NotMapped]
        public string RawFullName => $"{Name}{(string.IsNullOrEmpty(MiddleName) ? string.Empty : $" {MiddleName}")} {PaternalSurname} {MaternalSurname}";

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string CIPNumber { get; set; }

        public Guid ProfessionId { get; set; }
        public Profession Profession { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime StartTitle { get; set; }

        public DateTime CipDate { get; set; }

        public bool ValidationSunedu { get; set; }

        public bool CertiAdult { get; set; }

        public Uri DniUrl { get; set; }

        public Uri TitleUrl { get; set; }

        public Uri CipUrl { get; set; }

        public Uri CertiAdultUrl { get; set; }

        public Uri CapacitationUrl { get; set; }

        public int NumberOfExperiences { get; set; }

        public string Address { get; set; }

        public string Nacionality { get; set; }

        public Guid? CollegeId {get; set;}

        public College College { get; set; }

        public string University { get; set; }
    }
}
