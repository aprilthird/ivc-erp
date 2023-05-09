using IVC.PE.WEB.Areas.Bidding.ViewModels.SkillViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.ViewModels.SkillRenovationViewModels
{
    public class SkillRenovationViewModel
    {
        public Guid? Id { get; set; }

        public int SkillOrder { get; set; }

        public Guid SkillId { get; set; }
        public SkillViewModel Skill { get; set; }

        [Display(Name = "Fecha", Prompt = "Fecha")]
        public string CreateDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();

        [Display(Name = "Fecha Fin", Prompt = "Fecha Fin")]
        public string EndDate { get; set; }

        public Uri FileUrl { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }

        [Display(Name = "¿Es la última renovación?", Prompt = "¿Es la última renovación?")]
        public bool IsTheLast { get; set; }
        //Id a Renovar
        public Guid? SkillRenovationId { get; set; }
    }
}
