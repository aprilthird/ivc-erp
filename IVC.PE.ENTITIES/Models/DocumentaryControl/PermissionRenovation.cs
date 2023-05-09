using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.DocumentaryControl
{
    public class PermissionRenovation
    {
        public Guid Id { get; set; }

        public Guid PermissionId { get; set; }

        public Permission Permission { get; set; }

        public string AuthorizationNumber { get; set; }

        public Guid RenovationTypeId { get; set; }

        public RenovationType RenovationType { get; set; }

        public DateTime StartDate {get; set;}
        
        public DateTime EndDate { get; set; }
    
        public Uri FileUrl { get; set; }

        public bool Days15 { get; set; }

        public bool Days30 { get; set; }

        public int Order { get; set; }

        public bool IsTheLast { get; set; }
    }
}
