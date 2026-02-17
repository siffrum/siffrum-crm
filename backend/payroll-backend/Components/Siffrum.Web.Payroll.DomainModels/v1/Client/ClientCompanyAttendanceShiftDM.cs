using Siffrum.Web.Payroll.DomainModels.Base;

namespace Siffrum.Web.Payroll.DomainModels.v1.Client
{
    public class ClientCompanyAttendanceShiftDM : SiffrumPayrollDomainModelBase<int>
    {
        public DateTime ShiftFrom { get; set; }
        public DateTime ShiftTo { get; set; }
        public string ShiftName { get; set; }
        public string ShiftDescription { get; set; }
        public string PrimaryOfficeGeoCoordinatesLocation { get; set; }
        public string AllowedRaidus { get; set; }
        public string LocationBased { get; set; }
        public string AllowedIps { get; set; }

        [ForeignKey(nameof(ClientCompanyDetail))]
        public int? ClientCompanyDetailId { get; set; }
        public virtual ClientCompanyDetailDM ClientCompanyDetail { get; set; }
    }
}
