using Microsoft.AspNetCore.Mvc;

namespace Siffrum.Web.Payroll.API.Controllers.Common
{
    [Route("[controller]")]
    public partial class HealthController : ApiControllerRoot
    {

        public HealthController()
        {
        }


        #region Get Endpoints

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Index()
        {
            return Ok("I am up an running.");
        }

        #endregion Get Endpoints

    }
}
