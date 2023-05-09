using IVC.PE.CORE.Helpers;
using IVC.PE.ENTITIES.Models.Finance;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.ENTITIES.Models.Logistics;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.Models.General
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string PaternalSurname { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string MaternalSurname { get; set; }

        [NotMapped]
        public string FullName => $"{PaternalSurname} {MaternalSurname}, {Name}{(string.IsNullOrEmpty(MiddleName) ? string.Empty : $" {MiddleName}")}";

        [NotMapped]
        public string RawFullName => $"{Name}{(string.IsNullOrEmpty(MiddleName) ? string.Empty : $" {MiddleName}")} {PaternalSurname} {MaternalSurname}";

        public int WorkArea { get; set; }

        public Guid? WorkPositionId { get; set; }

        public WorkPosition WorkPosition { get; set; }

        public Guid? WorkAreaId { get; set; }

        public WorkArea WorkAreaEntity { get; set; }

        public Guid? WorkRoleId { get; set; }

        public WorkRole WorkRole { get; set; }

        public bool NewAccount { get; set; } = false;

        public bool NoEmail { get; set; } = false;

        public bool BelongsToMainOffice { get; set; } = false;

        public bool NewRoleSystem { get; set; } = false;

        public IEnumerable<ApplicationUserInterestGroup> UserInterestGroups { get; set; }

        public IEnumerable<ApplicationUserProject> UserProjects { get; set; }

        public IEnumerable<ApplicationUserRole> UserRoles { get; set; }

        public IEnumerable<RequestUser> RequestUsers { get; set; }

        public Uri SignatureUrl { get; set; }
    }
}
