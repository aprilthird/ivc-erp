using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.UserViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.Security.ViewModels.TrainingTopicViewModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace IVC.PE.WEB.Areas.Security.ViewModels.TrainingSessionViewModels
{
    public class TrainingSessionViewModel
    {
        [Display(Name = "Id")]
        public Guid? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Fecha", Prompt = "Fecha")]
        public string SessionDate { get; set; }

        public TrainingTopicViewModel TrainingTopic { get; set; }

        [Display(Name = "Categoría", Prompt = "Categoría")]
        public Guid TrainingCategoryId { get; set; }

        [Display(Name = "Tema", Prompt = "Tema")]
        public Guid TrainingTopicId { get; set; }

        public UserViewModel User { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Tutor", Prompt = "Tutor")]
        public string UserId { get; set; }

        public WorkFrontViewModel WorkFront { get; set; }

        [Display(Name = "Frente", Prompt = "Frente")]
        public Guid WorkFrontId { get; set; }
    }
}
