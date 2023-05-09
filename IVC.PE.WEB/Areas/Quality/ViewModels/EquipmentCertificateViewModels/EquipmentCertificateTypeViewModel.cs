using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.ViewModels.EquipmentCertificateViewModels
{
    public class EquipmentCertificateTypeViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Tipo de Certificado", Prompt = "Tipo de certificado")]
        public string CertificateTypeName { get; set; }

        [Display(Name = "Proyecto", Prompt = "Proyecto")]
        public Guid ProjectId { get; set; }

        public ProjectViewModel Project { get; set; }
        [Display(Name = "Color", Prompt = "Color")]
        public int Color { get; set; }

        public string ColorStr => ConstantHelpers.PillStyle.VALUES[Color];
    }
}
