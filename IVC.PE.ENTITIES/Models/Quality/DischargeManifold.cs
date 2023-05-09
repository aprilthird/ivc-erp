using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Quality
{
   public class DischargeManifold
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }

        public Project Project { get; set; }
        public string ProtocolNumber { get; set; }
        public Guid SewerManifoldId { get; set; }
        public SewerManifold SewerManifold { get; set; }


        //public string Section { get; set; }

        //public decimal LevelTopI { get; set; }

        //public decimal LevelBottomI { get; set; }

        //public decimal LevelTopJ { get; set; }

        //public decimal LevelBottomJ { get; set; }

        //public decimal LevelArrivalJ { get; set; }

        //public decimal LenghtBetweenAxisH { get; set; }

        public string Producer { get; set; }

        public string PipeBatch { get; set; }
        public string SecondPipeBatch { get; set; }
        public string ThridPipeBatch { get; set; }

        public string ForthPipeBatch { get; set; }

        public DateTime Leveling { get; set; } = DateTime.UtcNow;

        public DateTime? OpenZTest { get; set; } = DateTime.UtcNow;

        public DateTime? ClosedZTest { get; set; } = DateTime.UtcNow;

        public DateTime? MirrorTest { get; set; } = DateTime.UtcNow;

        public DateTime? BallTest { get; set; } = DateTime.UtcNow;

        public Guid? EquipmentCertificateId { get; set; }

        public Guid? EquipmentCertificate2Id { get; set; }


        [ForeignKey("EquipmentCertificateId")]
        public virtual EquipmentCertificate EquipmentCertificate { get; set; }
        
        [ForeignKey("EquipmentCertificate2Id")]
        public virtual EquipmentCertificate EquipmentCertificate2 { get; set; }



        public string BookPZO { get; set; }

        public string SeatPZC { get; set; }

        public string BookPZF { get; set; }

        public string SeatPZF { get; set; }

        public Uri FileUrl { get; set; }

        public Uri MirrorTestVideoUrl { get; set; }
        public Uri MonkeyBallTestVideoUrl { get; set; }
        public Uri ZoomTestVideoUrl { get; set; }
    }
}
