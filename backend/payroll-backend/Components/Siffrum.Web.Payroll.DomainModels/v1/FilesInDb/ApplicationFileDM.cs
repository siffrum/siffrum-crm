namespace Siffrum.Web.Payroll.DomainModels.v1.FilesInDb
{
    public class ApplicationFileDM : Base.SiffrumPayrollDomainModelBase<int>
    {
        public ApplicationFileDM()
        {
        }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileDescription { get; set; }

        ///NOTE : ONLY USE THIS METHOD IF THE FILE ID CAN BE SAVED IN DB TABLES DIFFERENTLY
        public byte[] FileBytes { get; set; }
    }
}
