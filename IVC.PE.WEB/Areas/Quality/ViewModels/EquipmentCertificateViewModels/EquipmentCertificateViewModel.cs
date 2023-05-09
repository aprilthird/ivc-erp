using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.EquipmentCertificateRenewalViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.ViewModels.EquipmentCertificateViewModels
{
    public class EquipmentCertificateViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Correlativo", Prompt = "Correlativo")]
        public string Correlative { get; set; }

        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }
        [Display(Name = "Marca", Prompt = "Marca")]
        public string Brand { get; set; }
        [Display(Name = "Modelo", Prompt = "Modelo")]
        public string Model { get; set; }
        [Display(Name = "Serie/Cod. ID", Prompt = "Serie/Cod. ID")]
        public string Serial { get; set; }
        [Display(Name = "Propietario", Prompt = "Propietario")]
        public Guid EquipmentCertificateOwnerId { get; set; }
        public EquipmentCertificateOwnerViewModel EquipmentCertificateOwner { get; set; }
        [Display(Name = "Tipo de equipo", Prompt = "Tipo de equipo")]
        public Guid EquipmentCertificateTypeId { get; set; }
        public EquipmentCertificateTypeViewModel EquipmentCertificateType{ get; set; }
        public Guid ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }
        [Display(Name = "Fecha de Ingreso", Prompt = "Fecha de Ingreso")]
        public string EntryDate { get; set; }

        public EquipmentCertificateRenewalViewModel EquipmentCertificateRenewal { get; set; }
        public IEnumerable<EquipmentCertificateRenewalViewModel> EquipmentCertificateRenewals { get; set; }

        public int NumberOfRenovations { get; set; }
    }
}
