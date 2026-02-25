using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Siffrum.Web.Payroll.API.Security;
using Siffrum.Web.Payroll.BAL.Notifications;
using Siffrum.Web.Payroll.ServiceModels.Constants;
using Siffrum.Web.Payroll.ServiceModels.Enums;
using Siffrum.Web.Payroll.ServiceModels.v1.Notifications;

namespace Siffrum.Web.Payroll.API.Controllers.Notifications
{
    [Route("api/v1/[controller]")]
    public class ClientNotificationController : ControllerBase
    {
        private readonly ClientNotificationProcess _clientNotificationProcess;

        public ClientNotificationController(ClientNotificationProcess clientNotificationProcess)
        {
            _clientNotificationProcess = clientNotificationProcess;
        }

        #region Get Endpoints

        [HttpGet("my")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientNotificationSM>>>> GetMyNotifications()
        {
            var listSM = await _clientNotificationProcess.GetMyNotifications();
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        [HttpGet("my/unread-count")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IntResponseRoot>>> GetMyUnreadCount()
        {
            var count = await _clientNotificationProcess.GetMyUnreadNotificationCount();
            return Ok(ModelConverter.FormNewSuccessResponse(new IntResponseRoot(count)));
        }

        #endregion

        #region Update Endpoints

        [HttpPut("my/{id}/read")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientNotificationSM>>> MarkAsRead(int id)
        {
            if (id <= 0)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(
                    DomainConstantsRoot.DisplayMessagesRoot.Display_IdInvalid,
                    ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            var resp = await _clientNotificationProcess.MarkAsRead(id);

            if (resp != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }

            return NotFound(ModelConverter.FormNewErrorResponse(
                DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved,
                ApiErrorTypeSM.NoRecord_NoLog));
        }

        #endregion

        #region Delete Endpoint

        [HttpDelete("my/{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> DeleteMyNotification(int id)
        {
            if (id <= 0)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(
                    DomainConstantsRoot.DisplayMessagesRoot.Display_IdInvalid,
                    ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            var resp = await _clientNotificationProcess.DeleteMyNotification(id);

            if (resp != null && resp.DeleteResult)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }

            return NotFound(ModelConverter.FormNewErrorResponse(
                resp?.DeleteMessage,
                ApiErrorTypeSM.NoRecord_NoLog));
        }

        #endregion
    }
}