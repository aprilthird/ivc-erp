using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class Provider
    {
        public Guid Id { get; set; }

        [Required]
        public string Code { get; set; }

        public string BusinessName { get; set; }

        public string Tradename { get; set; }

        public string RUC { get; set; }

        public string LegalAgent { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string CollectionAreaContactName { get; set; }

        public string CollectionAreaEmail { get; set; }

        public string CollectionAreaPhoneNumber { get; set; }

        public Guid? BankId { get; set; }

        public Bank Bank { get; set; }

        public int BankAccountType { get; set; }
        
        public string BankAccountNumber { get; set; }

        public string BankAccountCCI { get; set; }

        public Guid? ForeignBankId { get; set; }

        public Bank ForeignBank { get; set; }

        public int ForeignBankAccountType { get; set; }

        public int ForeignBankAccountCurrency { get; set; }

        public string ForeignBankAccountNumber { get; set; }

        public string ForeignBankAccountCCI { get; set; }

        public Guid? TaxBankId { get; set; }

        public Bank TaxBank { get; set; }

        public string TaxBankAccountNumber { get; set; }

        public int PropertyServiceType { get; set; }

        public string PropertyServiceCode { get; set; }

        public Guid? SupplyFamilyId { get; set; }

        public SupplyFamily SupplyFamily { get; set; }

        public Guid? SupplyGroupId { get; set; }

        public SupplyGroup SupplyGroup { get; set; }

        public IEnumerable<ProviderFile> ProviderFiles { get; set; }

        public IEnumerable<ProviderSupplyFamily> SupplyFamilies { get; set; }
        public IEnumerable<ProviderSupplyGroup> SupplyGroups { get; set; }

        //public string Observations { get; set; }

        //public double Score { get; set; }

        //public string Comments { get; set; }
    }
}
