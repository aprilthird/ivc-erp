using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Quality.ViewModels.EquipmentCertificateRenewalViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.EquipmentCertificateViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerBoxViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerManifoldViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.ViewModels.DischargeManifoldViewModels
{
    public class DischargeManifoldViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "# de protocolo", Prompt = "# de protocolo")]
        public string ProtocolNumber { get; set; }

        [Display(Name = "Tramos", Prompt = "Tramos")]
        public Guid SewerManifoldId { get; set; }

        public SewerManifoldFor01ViewModel SewerManifold { get; set; }
        [Display(Name = "# Serie Equipo Topográfico", Prompt = "# Serie Equipo Topográfico")]
        public Guid? EquipmentCertificateId { get; set; }

        [Display(Name = "# Serie Equipo Topográfico 2", Prompt = "# Serie Equipo Topográfico 2")]
        public Guid? EquipmentCertificate2Id { get; set; }

        public EquipmentCertificateViewModel EquipmentCertificate { get; set; }

        public EquipmentCertificateViewModel EquipmentCertificate2 { get; set; }


        [Display(Name = "Fabricante", Prompt = "Fabricante")]
        public string Producer { get; set; }
        [Display(Name = "Lote Tubería", Prompt = "Lote Tubería")]
        public string PipeBatch { get; set; }

        [Display(Name = "Lote Tubería 2 - Opcional", Prompt = "Lote Tubería 2 - opcional")]
        public string SecondPipeBatch { get; set; }
        [Display(Name = "Lote Tubería 3 - Opcional", Prompt = "Lote Tubería 3 - opcional")]
        public string ThirdPipeBatch { get; set; }
        [Display(Name = "Lote Tubería 4 - Opcional", Prompt = "Lote Tubería 4 - opcional")]
        public string FourthPipeBatch { get; set; }

        [Display(Name = "Nivelación", Prompt = "Nivelación")]
        public string Leveling { get; set; } = DateTime.UtcNow.Date.ToShortDateString();
        [Display(Name = "Prueba Z. Abierta", Prompt = "Prueba Z. Abierta")]
        public string OpenZTest { get; set; } = DateTime.UtcNow.Date.ToShortDateString();
        [Display(Name = "Prueba Z. Tapada", Prompt = "Prueba Z. Tapada")]
        public string ClosedZTest { get; set; } = DateTime.UtcNow.Date.ToShortDateString();
        [Display(Name = "Prueba Espejo", Prompt = "Prueba Espejo")]
        public string MirrorTest { get; set; } = DateTime.UtcNow.Date.ToShortDateString();
        [Display(Name = "Prueba Bola/Mandril", Prompt = "Prueba Bola/Mandril")]
        public string BallTest { get; set; }

        [Display(Name = "Libro PZA", Prompt = "Libro")]
        public string BookPZO { get; set; }
        [Display(Name = "Asiento PZA", Prompt = "Asiento")]
        public string SeatPZC { get; set; }
        [Display(Name = "Libro PZT", Prompt = "Libro")]
        public string BookPZF { get; set; }
        [Display(Name = "Asiento PZT", Prompt = "Asiento")]
        public string SeatPZF { get; set; }

        public Uri FileUrl { get; set; }

        public Uri MirrorTestVideoUrl { get; set; }
        public Uri MonkeyBallTestVideoUrl { get; set; }
        public Uri ZoomTestVideoUrl { get; set; }

        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }
        [Display(Name = "Video Espejo", Prompt = "Video Espejo")]
        public IFormFile VideoMirror { get; set; }
        [Display(Name = "Video Bolla/Mandrill", Prompt = "Video Bolla/Mandrill")]
        public IFormFile VideoMonkeyBall { get; set; }
        [Display(Name = "Video Zoom", Prompt = "Video Zoom")]
        public IFormFile VideoZoom { get; set; }



    }
}
