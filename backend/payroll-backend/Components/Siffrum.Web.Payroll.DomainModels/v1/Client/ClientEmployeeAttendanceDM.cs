using Siffrum.Web.Payroll.DomainModels.Base;
using Siffrum.Web.Payroll.DomainModels.Enums;
using Siffrum.Web.Payroll.DomainModels.v1.AppUsers;

namespace Siffrum.Web.Payroll.DomainModels.v1.Client
{
    public class ClientEmployeeAttendanceDM : SiffrumPayrollDomainModelBase<int>
    {
        public DateTime AttendanceDate { get; set; }
        public string? CheckIn { get; set; }
        public string? CheckOut { get; set; }
        public AttendanceStatusDM AttendanceStatus { get; set; }
        public string Location { get; set; }

        [ForeignKey(nameof(ClientUser))]
        public int ClientUserId { get; set; }

        [ForeignKey(nameof(ClientCompanyDetail))]
        public int ClientCompanyDetailId { get; set; }
        public virtual ClientCompanyDetailDM ClientCompanyDetail { get; set; }
        public virtual ClientUserDM ClientUser { get; set; }
    }
}
