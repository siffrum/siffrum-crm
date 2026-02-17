using System.Collections.Generic;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class ClientExcelFileSummarySM
    {
        public List<ClientEmployeeAttendanceExtendedUserSM> AttendanceSummary { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int TotalRecordsCount { get; set; }
        public int NumberOfRecordsNotAdded { get; set; }
        public int EmployeeRecordsAddedCount { get; set; }

    }
}

