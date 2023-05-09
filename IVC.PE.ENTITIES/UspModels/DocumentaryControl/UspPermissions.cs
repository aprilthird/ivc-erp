using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.DocumentaryControl
{
    [NotMapped]
    public class UspPermissions
    {
       public Guid BondAddId { get; set; }

        public Guid ProjectId { get; set; }

        public string ProjectAbbreviation { get; set; }

        public Guid ProjectFormulaId { get; set; }

        public string ProjectFormula { get; set; }

        public string PrincipalWay { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public Guid AuthorizingEntityId { get; set; }

        public string AuthorizingEntity { get; set; }

        public string Length { get; set; }

        public int NumberOfPermissions { get; set; }

        public Guid AuthorizationTypeId { get; set; }

        public string AuthorizationType { get; set; }

        public Guid BondRenovationId { get; set; }

        public int Order { get; set; }

        public DateTime StartDate { get; set; }
        public string StartDateString => $"{StartDate.ToDateString()}";
        public DateTime EndDate { get; set; }
        public string EndDateString => $"{EndDate.ToDateString()}";

        public string AuthorizationNumber { get; set; }

        public Guid RenovationTypeId { get; set; }

        public string RenovationType { get; set; }

        public int Validity { get; set; }

        public bool IsTheLast { get; set; }

        public Uri FileUrl { get; set; }

        public bool Days30 { get; set; }
        public bool Days15 { get; set; }
    }
}
