namespace Siffrum.Web.Payroll.DomainModels.v1.Client
{
    public class UserSettingDM : Base.SiffrumPayrollDomainModelBase<int>
    {
        [ForeignKey(nameof(ClientTheme))]
        public int ClientThemeId { get; set; }
        public virtual ClientThemeDM ClientTheme { get; set; }
    }
}
