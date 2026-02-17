namespace Siffrum.Web.Payroll.ServiceModels.v1.FilesInDb
{
    public class ApplicationFileSM : Base.SiffrumPayrollServiceModelBase<int>
    {
        public ApplicationFileSM()
        {
        }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileDescription { get; set; }

        ///NOTE : ONLY USE THIS METHOD IF THE FILE ID CAN BE SAVED IN DB TABLES DIFFERENTLY
        public byte[] FileBytes { get; set; }
    }
}
