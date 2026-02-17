using AutoMapper;
using BrainGateway.Authentication.Core;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Siffrum.Web.Payroll.API.AutoMapperBinding;
using Siffrum.Web.Payroll.API.Security;
using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.Config;
using Siffrum.Web.Payroll.ServiceModels.Enums;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1;

namespace Siffrum.Web.Payroll.API.Extensions
{
    public static class APIExtensions
    {
        public static void ConfigureCommonApplicationDependencies(this IServiceCollection services, IConfiguration baseConfiguration, APIConfiguration configObject)
        {
            #region Register Application Identification

            services.AddSingleton<ApplicationIdentificationRoot>((x) =>
            {
                var appIdentification = new ApplicationIdentificationRoot();
                baseConfiguration.GetRequiredSection("ApplicationIdentification").Bind(appIdentification);
                return appIdentification;
            });

            #endregion Application Identification

            #region Register Mapper

            services.AddSingleton<AutoMapper.IConfigurationProvider>(x =>
            {
                var config = new MapperConfiguration(
                    cfg =>
                    {
                        cfg.ConstructServicesUsing(t => x.GetService(t));
                        cfg.AddProfile(new AutoMapperDefaultProfile(x));
                    });
                return config;
            });
            services.AddSingleton<IMapper>(x =>
            {
                var config = x.GetRequiredService<AutoMapper.IConfigurationProvider>();
                return config.CreateMapper();
            });

            #endregion Register Mapper

            #region Register Logger

            #endregion Register Logger

            #region Register Context Accessor

            services.AddHttpContextAccessor();

            #endregion Register Context Accessor

            #region Register Base Configuration

            #endregion Register Base Configuration

            #region API Authentication

            //Register Auth
            services.Configure<RenoAuthenticationSchemeOptions>(x => x.JwtTokenSigningKey = configObject.JwtTokenSigningKey);

            //Auth // can use issuer constructor if we want seperate issuer for qa,reg and prod etc
            services.AddSingleton<JwtHandler>(x => new JwtHandler(configObject.JwtIssuerName));

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = APIBearerTokenAuthHandler.DefaultSchema;
            })
                .AddScheme<RenoAuthenticationSchemeOptions, APIBearerTokenAuthHandler>(APIBearerTokenAuthHandler.DefaultSchema, o => { })
                // Uncomment for Cookie Authentication , see CookieController for more info
                //.AddCookie((x) =>
                //{
                //    x.LoginPath = "/Cookie/ClientLogin";
                //    x.TicketDataFormat = new CustomSecureDateFormatter(JwtHandler, objAuthDecryptionConfiguration);
                //})
                ;

            services.AddSingleton<IPasswordEncryptHelper>((x) => new PasswordEncryptHelper(configObject.AuthTokenEncryptionKey, configObject.AuthTokenDecryptionKey));


            #endregion

            #region AutoRegister All Process

            services.AutoRegisterAllBALAsSelfFromBaseTypes<SiffrumPayrollBalBase>(ServiceLifetime.Scoped);

            #endregion AutoRegister All Process

            #region Register Swagger
            if (configObject.IsSwaggerEnabled)
            {
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                services.AddEndpointsApiExplorer();
                services.AddSwaggerGen(option =>
                {
                    option.SwaggerDoc("v1", new OpenApiInfo { Title = "SiffrumPayroll API", Version = "v1" });
                    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Enter Token Only (Without 'Bearer')",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
                    });
                    option.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });
                });
            }
            #endregion Register Swagger

            #region  To Enable Cors

            if (configObject.IsCorsEnabled)
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("AllowAllPolicy",
                        builder =>
                        {
                            builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            //.AllowCredentials()
                            ;
                        });
                });
            }

            #endregion  To Enable Cors

            #region LoggedInUser

            services.AddScoped<ILoginUserDetail>(x =>
            {
                var user = x.GetService<IHttpContextAccessor>().HttpContext.User;
                if (user != null && user.Identity.IsAuthenticated)
                {
                    if (user.IsInRole(RoleTypeSM.SuperAdmin.ToString()) || user.IsInRole(RoleTypeSM.SuperAdmin.ToString()))
                    {
                        var u = new LoginUserDetail();
                        u.DbRecordId = user.GetUserRecordIdFromCurrentUserClaims();
                        u.LoginId = user.Identity.Name;
                        u.UserType = Enum.Parse<RoleTypeSM>(user.GetUserRoleTypeFromCurrentUserClaims());
                        return u;
                    }
                    else if (user.IsInRole(RoleTypeSM.ClientAdmin.ToString()) || user.IsInRole(RoleTypeSM.ClientEmployee.ToString()))
                    {
                        var u = new LoginUserDetailWithCompany();
                        u.DbRecordId = user.GetUserRecordIdFromCurrentUserClaims();
                        u.LoginId = user.Identity.Name;
                        u.UserType = Enum.Parse<RoleTypeSM>(user.GetUserRoleTypeFromCurrentUserClaims());
                        u.CompanyRecordId = user.GetCompanyRecordIdFromCurrentUserClaims();
                        u.CompanyCode = user.GetCompanyCodeFromCurrentUserClaims();
                        return u;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                return new LoginUserDetail() { DbRecordId = 0, LoginId = "nullUser", UserType = RoleTypeSM.Unknown };
            });

            #endregion LoggedInUser

            #region Register Error Handler

            services.AddSingleton<ErrorLogHandlerRoot>(x =>
            {
                var appId = x.GetService<ApplicationIdentificationRoot>();
                var errorBal = new ErrorLogProcessRoot(configObject.ApiDbConnectionString, appId);
                return new ErrorLogHandlerRoot(configObject, errorBal, appId);
            });

            #endregion Register Error Handler

            #region Stripe
            Stripe.StripeConfiguration.ApiKey = configObject.StripeSettings.PrivateKey;
            #endregion
        }


        public static void ConfigureCommonInPipeline(this IApplicationBuilder app, APIConfiguration configObject)
        {
            //To Enable Cors
            if (configObject.IsCorsEnabled)
            {
                app.UseCors("AllowAllPolicy");
            }

            if (configObject.IsSwaggerEnabled)
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }

        #region Register Edmx Model
        public static IEdmModel GetEdmModel(this IServiceProvider serviceProvider)
        {
            ODataConventionModelBuilder builder = new();
            builder.EntitySet<DummySubjectSM>(nameof(DummySubjectSM));
            builder.EntitySet<DummyTeacherSM>(nameof(DummyTeacherSM));
            return builder.GetEdmModel();
        }

        #endregion Register Edmx Model
    }
}
