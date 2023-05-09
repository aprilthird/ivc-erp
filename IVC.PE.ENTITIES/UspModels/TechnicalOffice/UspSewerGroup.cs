using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.TechnicalOffice
{
    [NotMapped]
    public class UspSewerGroup
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public bool IsActive { get; set; }
        public string WorkFrontHeadCode { get; set; }
        public string WorkFrontHead { get; set; }
        public string ProviderBusinessName { get; set; }
        public string ProviderCollaborator { get; set; }
        public string ForemanEmployee { get; set; }
        public string ForemanWorker { get; set; }
        public Guid PeriodId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int Destination { get; set; }
        public int WorkStructure { get; set; }
        public int WorkComponent { get; set; }

        public string Responsable => ProviderCollaborator ?? ForemanEmployee ?? ForemanWorker ?? string.Empty;
    }
}
