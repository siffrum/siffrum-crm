using Microsoft.AspNetCore.Mvc;
using Siffrum.Web.Payroll.BAL.Common;
using Siffrum.Web.Payroll.ServiceModels.Constants;
using Siffrum.Web.Payroll.ServiceModels.v1.FilesInDb;

namespace Siffrum.Web.Payroll.API.Controllers.Common
{
    [Route("api/v1/[controller]")]
    public partial class ApplicationFileController : ApiControllerRoot<ApplicationFileSM>
    {
        private readonly ApplicationFileProcess _applicationFileProcess;
        public ApplicationFileController(ApplicationFileProcess process)
        {
            _applicationFileProcess = process;
        }

        #region Get Endpoints

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ApplicationFileSM>>> GetById(int id, bool getBytes = false)
        {
            var singleSM = await _applicationFileProcess.GetApplicationFileById(id, getBytes);
            if (singleSM != null)
            {
                return ModelConverter.FormNewSuccessResponse(singleSM);
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotFound, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion Get Endpoints

        #region Add/Update Endpoints

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ApplicationFileSM>>> Post() // keep blank for multipart
        {
            #region Check Request            

            (var apiRequest, var formFiles)
                = await base.TryReadApiRequestAsMultipart<ApplicationFileSM>(ensureAtLeastOneFile: true);

            //get ReqObject
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var addedSM = await _applicationFileProcess.AddApplicationFile(innerReq, formFiles.First());
            if (addedSM != null)
            {
                return CreatedAtAction(nameof(GetById), new
                {
                    id = addedSM.Id
                }, ModelConverter.FormNewSuccessResponse(addedSM));
            }
            else
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ApplicationFileSM>>> Put(int id) // intentionalyy no body used yet
        {
            #region Check Request            

            (var apiRequest, var formFiles)
                = await base.TryReadApiRequestAsMultipart<ApplicationFileSM>();

            //get ReqObject
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var resp = await _applicationFileProcess.UpdateApplicationFile(id, innerReq, formFiles?.FirstOrDefault());
            if (resp != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion Add/Update Endpoints

        #region Delete Endpoints

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> Delete(int id)
        {
            var resp = await _applicationFileProcess.DeleteApplicationFileById(id);
            if (resp != null && resp.DeleteResult)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(resp?.DeleteMessage, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion Delete Endpoints
    }
}
