using IVC.PE.CORE.Helpers;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class Employee
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string MiddleName { get; set; }
        
        [Required]
        public string PaternalSurname { get; set; }

        [Required]
        public string MaternalSurname { get; set; }

        public int DocumentType { get; set; } = ConstantHelpers.DocumentType.ID_CARD;

        [Required]
        public string Document { get; set; }

        [NotMapped]
        public string FullName => $"{PaternalSurname} {MaternalSurname}, {Name}{(string.IsNullOrEmpty(MiddleName) ? string.Empty : $" {MiddleName}")}";

        [NotMapped]
        public string RawFullName => $"{Name}{(string.IsNullOrEmpty(MiddleName) ? string.Empty : $" {MiddleName}")} {PaternalSurname} {MaternalSurname}";

        public DateTime? BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public Guid? BankId { get; set; }
        public Bank Bank { get; set; }

        public string BankAccount { get; set; }
        public string BankAccountCci { get; set; }

        public bool HaveHouseholdAllowance { get; set; }

        public int Gender { get; set; }
    }
}
