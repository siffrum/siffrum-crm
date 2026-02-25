using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1.Notifications;
using Siffrum.Web.Payroll.ServiceModels.Enums;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.Notifications;
using Siffrum.Web.Payroll.API.Services.Notifications;

namespace Siffrum.Web.Payroll.BAL.Notifications
{
    public class ClientNotificationProcess : SiffrumPayrollBalBase
    {
        #region --Properties--

        private readonly ILoginUserDetail _loginUserDetail;
        private readonly INotificationDeliveryService _notificationDeliveryService;

        #endregion

        #region --Constructor--

        public ClientNotificationProcess(
            IMapper mapper,
            ILoginUserDetail loginUserDetail,
            ApiDbContext apiDbContext,
            INotificationDeliveryService notificationDeliveryService)
            : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
            _notificationDeliveryService = notificationDeliveryService;
        }

        #endregion

        #region --Private Helper--

        private int GetCurrentCompanyId()
        {
            if (_loginUserDetail is LoginUserDetailWithCompany companyUser)
            {
                return companyUser.CompanyRecordId;
            }

            throw new SiffrumPayrollException(
                ApiErrorTypeSM.Fatal_Log,
                "Company context not available.",
                "Tenant context required.");
        }

        #endregion

        #region --Get--

        public async Task<List<ClientNotificationSM>> GetMyNotifications()
        {
            int companyId = GetCurrentCompanyId();
            int userId = _loginUserDetail.DbRecordId;

            var dm = await _apiDbContext.ClientNotifications
                .Where(x =>
                    x.ClientCompanyDetailId == companyId &&
                    x.UserId == userId &&
                    !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return _mapper.Map<List<ClientNotificationSM>>(dm);
        }

        public async Task<int> GetMyUnreadNotificationCount()
        {
            int companyId = GetCurrentCompanyId();
            int userId = _loginUserDetail.DbRecordId;

            return await _apiDbContext.ClientNotifications
                .Where(x =>
                    x.ClientCompanyDetailId == companyId &&
                    x.UserId == userId &&
                    !x.IsDeleted &&
                    !x.IsRead)
                .CountAsync();
        }

        #endregion

        #region --Add--

        public async Task<ClientNotificationSM> CreateNotification(
            int userId,
            string title,
            string message,
            NotificationTypeSM type,
            int? referenceId = null)
        {
            int companyId = GetCurrentCompanyId();

            var dm = new ClientNotificationDM
            {
                ClientCompanyDetailId = companyId,
                UserId = userId,
                Title = title,
                Message = message,
                NotificationType = (DomainModels.Enums.NotificationTypeDM)type,
                ReferenceId = referenceId,
                IsRead = false,
                IsSent = false,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                RetryCount = 0
            };

            await _apiDbContext.ClientNotifications.AddAsync(dm);
            await _apiDbContext.SaveChangesAsync();

            // 🔥 SEND PUSH IMMEDIATELY
            var user = await _apiDbContext.ClientUsers
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (!string.IsNullOrWhiteSpace(user?.OneSignalPlayerId))
            {
                try
                {
                    var sent = await _notificationDeliveryService.SendAsync(
                        user.OneSignalPlayerId,
                        title,
                        message);

                    if (sent)
                    {
                        dm.IsSent = true;
                        dm.SentAt = DateTime.UtcNow;
                    }
                    else
                    {
                        dm.RetryCount++;
                    }
                }
                catch
                {
                    dm.RetryCount++;
                }

                await _apiDbContext.SaveChangesAsync();
            }

            return _mapper.Map<ClientNotificationSM>(dm);
        }

        #endregion

        #region --Update--

        public async Task<ClientNotificationSM> MarkAsRead(int notificationId)
        {
            int companyId = GetCurrentCompanyId();
            int userId = _loginUserDetail.DbRecordId;

            var dm = await _apiDbContext.ClientNotifications
                .FirstOrDefaultAsync(x =>
                    x.Id == notificationId &&
                    x.ClientCompanyDetailId == companyId &&
                    x.UserId == userId &&
                    !x.IsDeleted);

            if (dm == null)
            {
                throw new SiffrumPayrollException(
                    ApiErrorTypeSM.Fatal_Log,
                    $"Notification not found: {notificationId}",
                    "Notification not found.");
            }

            dm.IsRead = true;
            await _apiDbContext.SaveChangesAsync();

            return _mapper.Map<ClientNotificationSM>(dm);
        }

        #endregion

        #region --Delete (Soft Delete)--

        public async Task<DeleteResponseRoot> DeleteMyNotification(int notificationId)
        {
            int companyId = GetCurrentCompanyId();
            int userId = _loginUserDetail.DbRecordId;

            var dm = await _apiDbContext.ClientNotifications
                .FirstOrDefaultAsync(x =>
                    x.Id == notificationId &&
                    x.ClientCompanyDetailId == companyId &&
                    x.UserId == userId &&
                    !x.IsDeleted);

            if (dm != null)
            {
                dm.IsDeleted = true;

                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Notification deleted successfully");
                }
            }

            return new DeleteResponseRoot(false, "Notification not found");
        }

        #endregion
    }
}