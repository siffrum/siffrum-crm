using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;
using Siffrum.Web.Payroll.API.Extensions;
using Siffrum.Web.Payroll.API.Filters;
using Siffrum.Web.Payroll.Config;
using Siffrum.Web.Payroll.DAL.Contexts;

namespace Siffrum.Web.Payroll.API
{
    public partial class Startup
    {
        public IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Bind config object
            var configObject = new APIConfiguration();
            var stripeSettings = new StripeSettings();

            Configuration.GetRequiredSection("APIConfiguration").Bind(configObject);
            Configuration.GetRequiredSection("StripeSettings").Bind(stripeSettings);
            configObject.StripeSettings = stripeSettings;

            // Validate JWT signing key size early to avoid cryptographic errors on startup
            if (string.IsNullOrWhiteSpace(configObject.JwtTokenSigningKey) || System.Text.Encoding.UTF8.GetByteCount(configObject.JwtTokenSigningKey) * 8 <= 256)
            {
                throw new InvalidOperationException("Configuration error: 'JwtTokenSigningKey' must be set and have more than 256 bits of entropy. Use a long random secret or store it in a secure secret store.");
            }

            services.AddSingleton<APIConfiguration>(x => configObject);
            services.ConfigureCommonApplicationDependencies(Configuration, configObject);

            RegisterAllThirdParties(services);

            var mvcBuilder = services.AddControllers(x =>
            {
                x.Filters.Add<APIExceptionFilter>();
            })
            .AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                opt.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.None;
                opt.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            });

            if (configObject.IsOdataEnabled)
            {
                mvcBuilder.AddOData((opt, x) =>
                {
                    opt.AddRouteComponents("v1", x.GetEdmModel())
                       .Filter()
                       .Select()
                       .Expand()
                       .OrderBy()
                       .SetMaxTop(100)
                       .SkipToken()
                       .Count();
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, APIConfiguration configObject)
        {
            app.ConfigureCommonInPipeline(configObject);

            if (!Directory.Exists(Path.Combine(env.WebRootPath, "website")))
                Directory.CreateDirectory(Path.Combine(env.WebRootPath, "website"));

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "website"))
            });

            app.Use(async (context, next) =>
            {
                context.Request.GetOrAddTracingId();
                await next.Invoke();
            });

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile(Path.Combine("website", "index.html"));
            });
        }

        private void RegisterAllThirdParties(IServiceCollection services)
        {
            #region PostgreSQL DB Context Setup

            services.AddDbContextPool<ApiDbContext>((provider, options) =>
            {
                options.UseNpgsql(
                    provider.GetService<APIConfiguration>().ApiDbConnectionString,
                    npgsqlOptions =>
                    {
                        // npgsqlOptions.MigrationsAssembly(typeof(Startup).Assembly.FullName);
                        // npgsqlOptions.EnableRetryOnFailure(3);
                    });

                options.EnableSensitiveDataLogging();
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
            });

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            #endregion PostgreSQL DB Context Setup
        }
    }
}
