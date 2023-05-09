using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.DocumentaryControl
{
    [NotMapped]
    public class UspLetter
    {
        public Guid LetterId { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public DateTime LetterDate { get; set; }
        public string LetterDateStr => LetterDate.ToDateString();
        public string Responsable { get; set; }
        public Guid? IssuerId { get; set; }
        public string IssuerName { get; set; }
        public int HasAnswers { get; set; }
        public string InterestGroupIds { get; set; }
        public IEnumerable<Guid> InterestGroupGuids => !string.IsNullOrEmpty(InterestGroupIds) ? InterestGroupIds.Split(',').OfType<string>().Select(x => Guid.Parse(x.Trim())).ToList() : new List<Guid>();
        public string InterestGroups { get; set; }
        public string ReferenceLetters { get; set; }
        public string DocCharacteristicIds { get; set; }
        public IEnumerable<Guid> DocCharacteristicGuids => !string.IsNullOrEmpty(DocCharacteristicIds) ? DocCharacteristicIds.Split(',').OfType<string>().Select(x => Guid.Parse(x.Trim())).ToList() : new List<Guid>();
        public string DocCharacteristics { get; set; }
        public Uri FileUrl { get; set; }
        //public string DocStyles { get; set; }
        //public IEnumerable<int> DocStyleInts => !string.IsNullOrEmpty(DocStyles) ? DocStyles.Split(',').OfType<string>().Where(x => int.TryParse(x, out int style)).Select(x => int.Parse(x.Trim())).ToList() : new List<int>();
    }
}
