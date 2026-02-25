using Siffrum.Web.Payroll.DomainModels.Enums;
using Siffrum.Web.Payroll.DomainModels.v1.AppUsers;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Web.Payroll.DomainModels.v1.Notifications
{
    [Index(nameof(ClientCompanyDetailId))]
    [Index(nameof(UserId))]
    [Index(nameof(IsSent))]
    public class ClientNotificationDM
    {
        [Key]
        public int Id { get; set; }

      
        [ForeignKey(nameof(ClientCompanyDetail))]
        public int ClientCompanyDetailId { get; set; }
        public virtual ClientCompanyDetailDM ClientCompanyDetail { get; set; }

        [ForeignKey(nameof(ClientUser))]
        public int UserId { get; set; }
        public virtual ClientUserDM ClientUser { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Message { get; set; }

        public NotificationTypeDM NotificationType { get; set; }

        public int? ReferenceId { get; set; }

        public bool IsRead { get; set; }

        public bool IsSent { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? SentAt { get; set; }

        public int RetryCount { get; set; }

        [MaxLength(2000)]
        public string LastError { get; set; }
    }
}