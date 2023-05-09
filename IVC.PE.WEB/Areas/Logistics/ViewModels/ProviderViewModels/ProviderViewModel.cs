using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BankViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels
{
    public class ProviderViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }

        [Display(Name = "Razón Social", Prompt = "Razón Social")]
        public string BusinessName { get; set; }

        [Display(Name = "Nombre Comercial", Prompt = "Nombre Comercial")]
        public string Tradename { get; set; }

        [Display(Name = "RUC", Prompt = "RUC")]
        public string RUC { get; set; }

        [Display(Name = "Representante Legal", Prompt = "Representante Legal")]
        public string LegalAgent { get; set; }

        [Display(Name = "Dirección", Prompt = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Teléfono", Prompt = "Teléfono")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Nombre Completo", Prompt = "Nombre Completo")]
        public string CollectionAreaContactName { get; set; }

        [Display(Name = "Correo ELectrónico", Prompt = "Correo ELectrónico")]
        public string CollectionAreaEmail { get; set; }

        [Display(Name = "Teléfono", Prompt = "Teléfono")]
        public string CollectionAreaPhoneNumber { get; set; }

        [Display(Name = "Banco", Prompt = "Banco")]
        public Guid? BankId { get; set; }

        public BankViewModel Bank { get; set; }

        [Display(Name = "Tipo de Cuenta", Prompt = "Tipo de Cuenta")]
        public int BankAccountType { get; set; }

        [Display(Name = "Número de Cuenta", Prompt = "Número de Cuenta")]
        public string BankAccountNumber { get; set; }

        [Display(Name = "CCI", Prompt = "CCI")]
        public string BankAccountCCI { get; set; }

        [Display(Name = "Banco", Prompt = "Banco")]
        public Guid? ForeignBankId { get; set; }

        public BankViewModel ForeignBank { get; set; }

        [Display(Name = "Tipo de Cuenta", Prompt = "Tipo de Cuenta")]
        public int ForeignBankAccountType { get; set; }

        [Display(Name = "Tipo de Moneda", Prompt = "Tipo de Moneda")]
        public int ForeignBankAccountCurrency { get; set; }

        [Display(Name = "Número de Cuenta", Prompt = "Número de Cuenta")]
        public string ForeignBankAccountNumber { get; set; }

        [Display(Name = "CCI", Prompt = "CCI")]
        public string ForeignBankAccountCCI { get; set; }

        [Display(Name = "Nombre", Prompt = "Nombre")]
        public Guid? TaxBankId { get; set; }

        public BankViewModel TaxBank { get; set; }

        [Display(Name = "Número de Cuenta", Prompt = "Número de Cuenta")]
        public string TaxBankAccountNumber { get; set; }

        [Display(Name = "Bien o Servicio", Prompt = "Bien o Servicio")]
        public int PropertyServiceType { get; set; }

        [Display(Name = "Código de Bien o Servicio", Prompt = "Código de Bien o Servicio")]
        public string PropertyServiceCode { get; set; }

        
        public Guid? SupplyFamilyId { get; set; }
        public SupplyFamilyViewModel SupplyFamily { get; set; }
        [Display(Name = "Familias", Prompt = "Familias")]
        public IEnumerable<Guid> SupplyFamilyIds { get; set; }
        public string SupplyFamilyNames { get; set; }


        public Guid? SupplyGroupId { get; set; }
        public SupplyGroupViewModel SupplyGroup { get; set; }
        [Display(Name = "Grupos", Prompt = "Grupos")]
        public IEnumerable<Guid?> SupplyGroupIds { get; set; }
        public string SupplyGroupNames { get; set; }
    }
}
