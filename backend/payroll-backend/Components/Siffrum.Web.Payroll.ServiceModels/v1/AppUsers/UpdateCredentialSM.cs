namespace Siffrum.Web.Payroll.ServiceModels.v1.AppUsers
{
    public class UpdateCredentialSM
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public int UserId { get; set; }
    }
}
