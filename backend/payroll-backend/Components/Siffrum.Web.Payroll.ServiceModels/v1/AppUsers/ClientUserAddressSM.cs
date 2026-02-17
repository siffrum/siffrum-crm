using Siffrum.Web.Payroll.ServiceModels.Enums;

namespace Siffrum.Web.Payroll.ServiceModels.v1.AppUsers
{
    public class ClientUserAddressSM : Base.SiffrumPayrollServiceModelBase<int>
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PinCode { get; set; }
        public ClientUserAddressTypeSM ClientUserAddressType { get; set; }
        public int ClientUserId { get; set; }
        public int ClientCompanyDetailId { get; set; }
    }
}
