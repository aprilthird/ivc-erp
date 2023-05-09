using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.DocumentaryControl
{
    public class Permission
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        public Guid ProjectFormulaId { get; set; }

        public ProjectFormula ProjectFormula { get; set; }

        public string PrincipalWay { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public Guid AuthorizingEntityId { get; set; }

        public AuthorizingEntity AuthorizingEntity { get; set; }

        public string Length { get; set; }

        public Guid AuthorizationTypeId { get; set; }

        public AuthorizationType AuthorizationType { get; set;}

        public int NumberOfPermissions { get; set; }   
    }
}
