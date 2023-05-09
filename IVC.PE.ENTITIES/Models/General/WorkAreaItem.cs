using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.General
{
    public class WorkAreaItem
    {
        public Guid Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string NormalizedName { get; set; }
        
        public Guid WorkAreaId { get; set; }
        
        public WorkArea WorkArea { get; set; }
        
        public Guid? ParentId { get; set; }
        
        public WorkAreaItem Parent { get; set; }

        public bool IsItemGroup { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public ApplicationRole Role { get; set; }

        public string RoleId { get; set; }

        public IEnumerable<WorkRoleItem> WorkRoleItems { get; set; }

        public IEnumerable<WorkAreaItem> WorkAreaItems { get; set; }
    }
}
