using Siffrum.Web.Payroll.ServiceModels.Base;
using Siffrum.Web.Payroll.ServiceModels.Enums;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class ClientEmployeeAttendanceSM : SiffrumPayrollServiceModelBase<int>
    {
        public DateTime AttendanceDate { get; set; }
        public string? CheckIn { get; set; }
        public string? CheckOut { get; set; }
        public AttendanceStatusSM AttendanceStatus { get; set; }
        public string Location { get; set; }
        public int ClientUserId { get; set; }
        public int ClientCompanyDetailId { get; set; }
        public string ErrorMessageInUpload { get; set; }

    }
}
