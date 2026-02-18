using AutoMapper;
using Siffrum.Web.Payroll.DomainModels.Base;
using Siffrum.Web.Payroll.DomainModels.v1.AppUsers;
using Siffrum.Web.Payroll.DomainModels.v1.AppUsers.Login;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using Siffrum.Web.Payroll.DomainModels.v1.License; // ✅ ADDED (needed for license mapping)

using Siffrum.Web.Payroll.ServiceModels.Base;
using Siffrum.Web.Payroll.ServiceModels.v1.AppUsers;
using Siffrum.Web.Payroll.ServiceModels.v1.AppUsers.Login;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;
using Siffrum.Web.Payroll.ServiceModels.v1.License; // ✅ ADDED (needed for license mapping)

namespace Siffrum.Web.Payroll.API.AutoMapperBinding
{

    public class AutoMapperMappings
    {
        public static IMapper RegisterObjectMappings(string dmNameExtension = "DM", string smNameExtension = "SM")
        {
            var config = new MapperConfiguration(cfg =>
            {
                //cfg.CreateMap<DummySubjectDM, DummySubjectSM>()
                cfg.CreateMap<LoginUserDM, LoginUserSM>().ReverseMap();
                cfg.CreateMap<ClientEmployeeBankDetailDM, ClientEmployeeBankDetailSM>().ReverseMap();
                cfg.CreateMap<ClientCompanyDetailDM, ClientCompanyDetailSM>().ReverseMap();
                cfg.CreateMap<ClientCompanyAddressDM, ClientCompanyAddressSM>().ReverseMap();
                cfg.CreateMap<ClientUserDM, ClientUserSM>().ReverseMap();
                cfg.CreateMap<ClientUserAddressDM, ClientUserAddressSM>().ReverseMap();
                cfg.CreateMap<DocumentsDM, DocumentsSM>().ReverseMap();
                cfg.CreateMap<ClientGenericPayrollComponentDM, ClientGenericPayrollComponentSM>().ReverseMap();
                cfg.CreateMap<ClientEmployeeAdditionalReimbursementLogDM, ClientEmployeeAdditionalReimbursementLogSM>().ReverseMap();
                cfg.CreateMap<ClientEmployeeCTCDetailDM, ClientEmployeeCTCDetailSM>().ReverseMap();
                cfg.CreateMap<ClientEmployeePayrollComponentDM, ClientEmployeePayrollComponentSM>().ReverseMap();
                cfg.CreateMap<ClientEmployeeDocumentDM, ClientEmployeeDocumentSM>().ReverseMap();
                cfg.CreateMap<ClientEmployeeLeaveDM, ClientEmployeeLeaveSM>().ReverseMap();
                cfg.CreateMap<PayrollTransactionDM, PayrollTransactionSM>().ReverseMap();
                cfg.CreateMap<PermissionDM, PermissionSM>().ReverseMap();
                cfg.CreateMap<CompanyModulesDM, CompanyModulesSM>().ReverseMap();

            

                // create auto mapping from DM to SM with same names
                var mapResp = cfg.RegisterAutoMapperFromDmToSm<SiffrumPayrollDomainModelBase<object>, SiffrumPayrollServiceModelBase<object>>();
                Console.WriteLine("AutoMappings Success: " + mapResp.SuccessfullMaps.Count);
                Console.WriteLine("AutoMappings Error: " + mapResp.UnsuccessfullPaths.Count);
            });

            IMapper mapper = config.CreateMapper();
            return mapper;
        }
    }
}
