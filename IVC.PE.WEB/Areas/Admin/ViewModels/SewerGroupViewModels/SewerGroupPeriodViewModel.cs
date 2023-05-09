using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectCollaboratorViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontHeadViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels
{
    public class SewerGroupPeriodViewModel
    {
        public Guid? Id { get; set; }
        public Guid SewerGroupId { get; set; }
        public SewerGroupViewModel SewerGroup { get; set; }

        [Display(Name = "Frentes", Prompt = "Frentes")]
        public IEnumerable<Guid> WorkFrontIds { get; set; }
        public Guid? WorkFrontId { get; set; }
        public WorkFrontViewModel WorkFront { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Jefe de Frente", Prompt = "Jefe de Frente")]
        public Guid? WorkFrontHeadId { get; set; }
        public WorkFrontHeadViewModel WorkFrontHead { get; set; }
        
        [Display(Name = "Destino", Prompt = "Destino")]
        public int Destination { get; set; }
        
        [Display(Name = "Componente de Obra", Prompt = "Componente de Obra")]
        public int WorkComponent { get; set; }
        
        [Display(Name = "Estructura", Prompt = "Estructura")]
        public int WorkStructure { get; set; }
        
        [Display(Name = "Colaborador", Prompt = "Colaborador")]
        public Guid? ProviderId { get; set; }
        public ProviderViewModel Provider { get; set; }
        
        [Display(Name = "Responsable", Prompt = "Responsable")]
        public Guid? ProjectCollaboratorId { get; set; }
        public ProjectCollaboratorViewModel ProjectCollaborator { get; set; }
        
        [Display(Name = "Responsable", Prompt = "Responsable")]
        public Guid? ForemanId { get; set; }
        public string EmployeeWorkerName { get; set; }

        
        
        [Display(Name = "Vigencia Inicio:", Prompt = "Vigencia Inicio:")]
        public string DateStart { get; set; }
        
        [Display(Name = "Vigencia Fin:", Prompt = "Vigencia Fin:")]
        public string DateEnd { get; set; }
        public bool IsActive { get; set; }
    }
}
