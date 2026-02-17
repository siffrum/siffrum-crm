using Siffrum.Web.Payroll.ServiceModels.Base;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class ClientCompanyAttendanceShiftSM : SiffrumPayrollServiceModelBase<int>
    {
        public DateTime ShiftFrom { get; set; }
        public DateTime ShiftTo { get; set; }
        public string ShiftName { get; set; }
        public string ShiftDescription { get; set; }
        public string PrimaryOfficeGeoCoordinatesLocation { get; set; }
        public string AllowedRaidus { get; set; }
        public string LocationBased { get; set; }
        public string AllowedIps { get; set; }
        public int ClientCompanyDetailId { get; set; }
    }
}
