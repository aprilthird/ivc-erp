using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.Biddings
{
    [NotMapped]
    public class UspProfessionalSecondExcel
    {
        public Guid BusinessId { get; set; }

        public string Business { get; set; }

        public Guid BiddingWorkId { get; set; }

        public string BiddingWork { get; set; }

        public Guid PositionId { get; set; }

        public string Position { get; set; }

        public DateTime StartDate { get; set; }
        public string StartDateString => $"{StartDate.ToDateString()}";

        public DateTime EndDate { get; set; }
        public string EndDateString => $"{EndDate.ToDateString()}";
        public int Dif { get; set; }

        public string Observations { get; set; }

        public int Order { get; set; }
    }
}
