using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.General
{
    public class WorkArea
    {
        public Guid Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string NormalizedName { get; set; }
        
        public int IntValue { get; set; }
   
        public IEnumerable<ApplicationUser> Users { get; set; }

        public IEnumerable<WorkAreaItem> WorkAreaItems { get; set; }
    }
}
