namespace Siffrum.Ecom.DomainModels.v1.Helpers
{
    public static class ReturnStatusList
    {
        public const int Pending = 1;
        public const int Approved = 2;
        public const int Rejected = 3;
        public const int DeliveryBoyAssigned = 4;
        public const int OutForPickup = 5;
        public const int ReceivedFromCustomer = 6;
        public const int Cancelled = 7;
        public const int ReturnToSeller = 8;

        public const string RequestPending = "Request Pending";
        public const string RequestApproved = "Request Approved";
        public const string RequestRejected = "Request Rejected";
        public const string DeliveryBoyAssignedText = "Delivery Boy Assigned";
        public const string OutForPickupText = "Out for Pickup";
        public const string ReceivedFromCustomerText = "Received from Customer";
        public const string CancelledText = "Cancelled";
        public const string ReturnToSellerText = "Return to Seller";

        public static string GetStatusName(int status)
        {
            return status switch
            {
                Pending => RequestPending,
                Approved => RequestApproved,
                Rejected => RequestRejected,
                DeliveryBoyAssigned => DeliveryBoyAssignedText,
                OutForPickup => OutForPickupText,
                ReceivedFromCustomer => ReceivedFromCustomerText,
                Cancelled => CancelledText,
                ReturnToSeller => ReturnToSellerText,
                _ => "Unknown Status"
            };
        }
    }

}
