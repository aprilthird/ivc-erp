using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.BINDINGRESOURCES.Areas.Security
{
    public class RacsResourceModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public Guid ProjectId { get; set; }
        public string ReportDate { get; set; }
        public string ApplicationUserId { get; set; }
        public string LiftResponsibleId { get; set; }
        public Guid? WorkFrontId { get; set; }
        public Guid? SewerGroupId { get; set; }
        public Uri SignatureUrl { get; set; }
        public byte[] SignatureArray { get; set; }

        //Substandar Condition
        public bool IdentifiesSC { get; set; }
        public string DescriptionIdentifiesSC { get; set; }
        public bool SCQ01 { get; set; }
        public bool SCQ02 { get; set; }
        public bool SCQ03 { get; set; }
        public bool SCQ04 { get; set; }
        public bool SCQ05 { get; set; }
        public bool SCQ06 { get; set; }
        public bool SCQ07 { get; set; }
        public bool SCQ08 { get; set; }
        public bool SCQ09 { get; set; }
        public bool SCQ10 { get; set; }
        public bool SCQ11 { get; set; }
        public bool SCQ12 { get; set; }
        public bool SCQ13 { get; set; }
        public bool SCQ14 { get; set; }
        public bool SCQ15 { get; set; }
        public bool SCQ16 { get; set; }
        public bool SCQ17 { get; set; }
        public bool SCQ18 { get; set; }
        public bool SCQ19 { get; set; }
        public bool SCQ20 { get; set; }
        public bool SCQ21 { get; set; }
        public bool SCQ22 { get; set; }
        public bool SCQ23 { get; set; }
        public bool SCQ24 { get; set; }
        public bool SCQ25 { get; set; }
        public bool SCQ26 { get; set; }
        public bool SCQ27 { get; set; }
        public bool SCQ28 { get; set; }
        public bool SCQ29 { get; set; }
        public string SpecifyConditions { get; set; }

        //Substandar Act
        public bool IdentifiesSA { get; set; }
        public string DescriptionIdentifiesSA { get; set; }
        public bool SAQ01 { get; set; }
        public bool SAQ02 { get; set; }
        public bool SAQ03 { get; set; }
        public bool SAQ04 { get; set; }
        public bool SAQ05 { get; set; }
        public bool SAQ06 { get; set; }
        public bool SAQ07 { get; set; }
        public bool SAQ08 { get; set; }
        public bool SAQ09 { get; set; }
        public bool SAQ10 { get; set; }
        public bool SAQ11 { get; set; }
        public bool SAQ12 { get; set; }
        public bool SAQ13 { get; set; }
        public bool SAQ14 { get; set; }
        public bool SAQ15 { get; set; }
        public bool SAQ16 { get; set; }
        public bool SAQ17 { get; set; }
        public bool SAQ18 { get; set; }
        public bool SAQ19 { get; set; }
        public bool SAQ20 { get; set; }
        public bool SAQ21 { get; set; }
        public bool SAQ22 { get; set; }
        public bool SAQ23 { get; set; }
        public bool SAQ24 { get; set; }
        public bool SAQ25 { get; set; }
        public bool SAQ26 { get; set; }
        public bool SAQ27 { get; set; }
        public bool SAQ28 { get; set; }
        public bool SAQ29 { get; set; }
        public bool SAQ30 { get; set; }
        public bool SAQ31 { get; set; }
        public bool SAQ32 { get; set; }
        public bool SAQ33 { get; set; }
        public bool SAQ34 { get; set; }
        public bool SAQ35 { get; set; }
        public bool SAQ36 { get; set; }

        public string SpecifyActs { get; set; }

        //Immediate Correction
        public bool ICQ01 { get; set; }
        public bool ICQ02 { get; set; }
        public bool ICQ03 { get; set; }
        public bool ICQ04 { get; set; }
        public bool ICQ05 { get; set; }
        public string SpecifyAppliedCorrections { get; set; }
        public string SpecifyAnotherAlternative { get; set; }
        public Uri ObservationImageUrl { get; set; }
        public byte[] ObservationImageArray { get; set; }
        //RACS Lifting
        public string LiftingObservations { get; set; }
        public Uri LiftingImageUrl { get; set; }
        public byte[] LiftingImageArray { get; set; }
        public int Status { get; set; }
    }

    public class RacsListResourceModel
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string ReportDate { get; set; }

        public string SewerGroupCode { get; set; }

        public int Status { get; set; }
        public string StatusStr { get; set; }

        public string ReportUser { get; set; }
    }

    public class RacsToLift
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public Uri ObservationImageUrl { get; set; }

        //RACS Lifting
        public string LiftingObservations { get; set; }
        public Uri LiftingImageUrl { get; set; }
        public byte[] LiftingImageArray { get; set; }
        public int Status { get; set; }
    }
}
