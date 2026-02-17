namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class ClientEmployeeAttendanceExtendedUserSM : ClientEmployeeAttendanceSM
    {
        public string UserName { get; set; }
        public string EmployeeCode { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public int LeaveCount { get; set; }
    }
}
