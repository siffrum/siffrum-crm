using System.Collections.Generic;
using System.Data;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{  
    public class SQLReportResponseModel
    {
        public int Id { get; set; }
        public string ReportName { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<SQLReportColumns> DataColumns { get; set; }
        public DataTable ReportData { get; set; }
    }
    public class SQLReportColumns
    {
        public int ColumnIndex { get; set; }
        public string ColumnName { get; set; }
    }
}
