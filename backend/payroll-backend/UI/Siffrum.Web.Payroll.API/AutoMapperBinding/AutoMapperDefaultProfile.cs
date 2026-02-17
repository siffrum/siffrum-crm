using AutoMapper;

using Siffrum.Web.Payroll.DomainModels.Base;
using Siffrum.Web.Payroll.DomainModels.v1.AppUsers.Login;
using Siffrum.Web.Payroll.ServiceModels.Base;
using Siffrum.Web.Payroll.ServiceModels.v1.AppUsers.Login;

namespace Siffrum.Web.Payroll.API.AutoMapperBinding
{
    public class AutoMapperDefaultProfile : Profile
    {
        public AutoMapperDefaultProfile(IServiceProvider serviceProvider)
        {
            ApplicationSpecificMappings();


            //this.CreateMap<DummySubjectDM, DummySubjectSM>()
            //.ForMember(dst => dst.CreatedOnLTZ, opts => opts.MapFrom(src => DateExtensions.ConvertFromUTCToSystemTimezone(src.CreatedOnUTC)))
            //.ReverseMap();

            //this.CreateMap(typeof(DummySubjectDM), typeof(DummySubjectSM))
            //    .ForMember(nameof(DummySubjectSM.CreatedOnLTZ), opt =>
            //    {
            //        opt.MapFrom("CreatedOnUTC");
            //    });            

            // create auto mapping from DM to SM with same names
            var mapResp = this.RegisterAutoMapperFromDmToSm<SiffrumPayrollDomainModelBase<object>, SiffrumPayrollServiceModelBase<object>>();

            Console.WriteLine("AutoMappings Success: " + mapResp.SuccessfullMaps.Count);
            Console.WriteLine("AutoMappings Error: " + mapResp.UnsuccessfullPaths.Count);

            // serviceProviderUsage here
            //.ForMember(
            //    dest => dest.PropertyName,
            //    opt => opt.MapFrom(
            //        s => serviceProvider.GetService<ILanguage>().Language == "en-US"
            //            ? s.PropertyEnglishName
            //            : s.PropertyArabicName));
        }


        private void ApplicationSpecificMappings()
        {
            this.CreateMap<LoginUserDM, LoginUserSM>();
        }
    }
}
