using Siffrum.Web.Payroll.ServiceModels.Enums;
using System.Collections.Generic;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class SQLReportDataModelSM
    {
        public int? ClientCompanyDetailId { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public List<SQLReportCellSM> ReportDataColumns { get; set; }
        public List<SQLReportRowsSM> ReportDataRows { get; set; }
    }
    public class SQLReportCellSM
    {
        public int CellColumnIndex { get; set; }
        public string CellColumnName { get; set; }
        public CellDataType CellDataType { get; set; }
        public object CellValue { get; set; }
    }
    public class SQLReportRowsSM
    {
        public List<SQLReportCellSM> ReportDataCells { get; set; }

    }
}
