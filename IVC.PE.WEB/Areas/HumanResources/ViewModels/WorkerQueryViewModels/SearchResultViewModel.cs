using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerQueryViewModels
{
    public class SearchResultViewModel
    {
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public int DocumentType { get; set; } = ConstantHelpers.DocumentType.ID_CARD;
        public string Document { get; set; }
        public string FullName => $"{PaternalSurname} {MaternalSurname}, {Name}{(string.IsNullOrEmpty(MiddleName) ? string.Empty : $" {MiddleName}")}";
        public string RawFullName => $"{Name}{(string.IsNullOrEmpty(MiddleName) ? string.Empty : $" {MiddleName}")} {PaternalSurname} {MaternalSurname}";
        public DateTime? BirthDate { get; set; }
        public DateTime? EntryDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime? EmailConfirmationDateTime { get; set; }
        public Uri PhotoUrl { get; set; }
        public int Gender { get; set; }
        public int Category { get; set; }
        public IEnumerable<SearchDetailViewModel> Details { get; set; }
    }

    public class SearchDetailViewModel
    {
        public DateTime? EntryDate { get; set; }
        public Guid ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }
        public string PensionFundUniqueIdentificationCode { get; set; }
        public int Category { get; set; }
        public int Origin { get; set; }
        public int Workgroup { get; set; }
        public int LaborRegimen { get; set; }
        public bool HasSctr { get; set; }
        public int SctrHealthType { get; set; }
        public int SctrPensionType { get; set; }
        public DateTime? CeaseDate { get; set; }
        public bool IsActive { get; set; }


        public class ProjectViewModel
        {
            public string Name { get; set; }

            public Uri LogoUrl { get; set; }

            public string Description { get; set; }
        }
    }
}