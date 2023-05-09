using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class PreRequestUser
    {
        public Guid Id { get; set; }
        public Guid PreRequestId { get; set; }

        public PreRequest PreRequest { get; set; }

        [Required]
        [MaxLength(256)]
        public string UserId { get; set; }

        public string FullName { get; set; }
    }
}
