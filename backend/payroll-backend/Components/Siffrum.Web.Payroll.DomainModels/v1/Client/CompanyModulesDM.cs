using Siffrum.Web.Payroll.DomainModels.Base;
using Siffrum.Web.Payroll.DomainModels.Enums;

namespace Siffrum.Web.Payroll.DomainModels.v1.Client
{
    public class CompanyModulesDM : SiffrumPayrollDomainModelBase<int>
    {
        public string ModuleValue { get; set; }
        public string Description { get; set; }
        public ModuleNameDM ModuleName { get; set; }
        public bool? IsEnabled { get; set; }
        public bool? StandAlone { get; set; }
        //public bool? HasCompanyDetailModule { get; set; }
        //public bool? HasCompanyAddressModule { get; set; }
        //public bool? HasEmployeeModule { get; set; }
        //public bool? HasEmployeeAddressModule { get; set; }
        //public bool? HasLeaveModule { get; set; }
        //public bool? HasPayrollTransactionModule { get; set; }
        //public bool? HasEmployeeAdditionalReimbursementLogsModule { get; set; }
        //public bool? HasEmployeeBankDetailModule { get; set; }
        //public bool? HasEmployeeCTCDetailModule { get; set; }
        //public bool? HasEmployeeDocumentsModule { get; set; }
        //public bool? HasGenericPayrollComponentModule { get; set; }
        //public bool? HasCompanyAccountTransactionsModule { get; set; }
        //public bool? HasDocumentsModule { get; set; }
        //public bool? HasGenerateLetters { get; set; }
        //public bool? HasAttendance { get; set; }
        //public bool? HasReports { get; set; }
        //public bool? HasSettings { get; set; }
        //public bool? HasCompanyLetters { get; set; }
        //public bool? HasEmployeeDirectory { get; set; }
        //public bool? HasCompanyAttendanceShift { get; set; }

        //[ForeignKey(nameof(ClientCompanyDetail))]
        //public int ClientCompanyDetailId { get; set; }
        //public virtual ClientCompanyDetailDM ClientCompanyDetail { get; set; }

    }
}
