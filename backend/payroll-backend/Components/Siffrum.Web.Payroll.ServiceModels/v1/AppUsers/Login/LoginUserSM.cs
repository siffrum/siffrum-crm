using Siffrum.Web.Payroll.ServiceModels.Enums;

namespace Siffrum.Web.Payroll.ServiceModels.v1.AppUsers.Login
{
    public class LoginUserSM : Base.SiffrumPayrollServiceModelBase<int>
    {
        public LoginUserSM()
        {
        }
        public RoleTypeSM RoleType { get; set; }
        public string LoginId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }

        [IgnorePropertyOnWrite(AutoMapConversionType.Dm2SmOnly)]
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePicturePath { get; set; }

        //[ConvertFilePathToUri(SourcePropertyName = nameof(ProfilePicturePath))]
        //public string ProfilePicturePathUrl { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public LoginStatusSM LoginStatus { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}

