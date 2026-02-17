using System.Collections.Generic;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class ClientExcelFileResponseSM
    {
        public Dictionary<string, int> ExcelFieldMapping { get; set; }
        public int HeaderRow { get; set; }
        public string FileName { get; set; }
    }
}
