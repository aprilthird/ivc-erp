using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Security
{
    public class RacsSummary
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public string RacsCode { get; set; }
        public int RacsCount { get; set; }
        public string VersionCode { get; set; }
    }
}
