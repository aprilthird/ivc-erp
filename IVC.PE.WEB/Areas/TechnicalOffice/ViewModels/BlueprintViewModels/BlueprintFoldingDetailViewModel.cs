using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BlueprintViewModels
{
    public class BlueprintFoldingDetailViewModel
    {
        public Guid? Id { get; set; }
        public Guid BlueprintFoldingId { get; set; }
        [Display(Name = "Fecha de Entrega", Prompt = "Fecha de Entrega")]
        public string DateType { get; set; }

        [Display(Name = "Encargado", Prompt = "Encargado")]

        public string UserId { get; set; }

        public string UserName { get; set; }
    }
}
