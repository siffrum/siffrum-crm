using Microsoft.AspNetCore.Mvc;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DAL.Seeds;

namespace Siffrum.Web.Payroll.API.Controllers.Common
{
    [ApiController]
    [Route("[controller]")]
    public partial class DatabaseSeedController : ApiControllerRoot
    {
        private readonly ApiDbContext _apiDbContext;
        private readonly IPasswordEncryptHelper _passwordEncryptHelper;

        public DatabaseSeedController(ApiDbContext context, IPasswordEncryptHelper passwordEncryptHelper)
        {
            _apiDbContext = context;
            _passwordEncryptHelper = passwordEncryptHelper;
        }

        [HttpGet]
        [Route("Init")]
        public async Task<IActionResult> Get()
        {
            try
            {
                DatabaseSeeder<ApiDbContext> databaseSeeder = new DatabaseSeeder<ApiDbContext>();
                var retVal = await databaseSeeder.SetupDatabaseWithTestData(_apiDbContext, (x) => _passwordEncryptHelper.ProtectAsync<string>(x).Result);
                
                if (retVal)
                {
                    return Ok(new { success = true, message = "Database seeded successfully" });
                }
                else
                {
                    var userCount = _apiDbContext.ApplicationUsers.Count();
                    return Ok(new { success = false, message = $"Database seeding skipped. Database already contains {userCount} user(s). Clear the database first if you want to re-seed." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error seeding database", error = ex.Message });
            }
        }
    }
}
