using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.SewerManifoldFor24ViewModels
{
    public class SewerManifoldFor24ThirdPartViewModel
    {
        public Guid? Id { get; set; }
        public Guid SewerManifoldFor24SecondPartId { get; set; }
        public SewerManifoldFor24SecondPartViewModel SewerManifoldFor24SecondPart { get; set; }
        public int ActionTaken { get; set; }
        public bool PreventiveCorrectiveAction { get; set; }
        public string ClosingDate { get; set; }
        public Uri FileUrl { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }
    }
}
