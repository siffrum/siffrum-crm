namespace Siffrum.Ecom.DomainModels.v1.Helpers
{
    public static class OrderStatusList
    {
        public const int PaymentPending = 1;
        public const int Received = 2;
        public const int Processed = 3;
        public const int Shipped = 4;
        public const int OutForDelivery = 5;
        public const int Delivered = 6;
        public const int Cancelled = 7;
        public const int Returned = 8;

        public const int SelfPickupPending = 9;
        public const int SelfPickupReady = 10;
        public const int SelfPickupPicked = 11;

        public const string OrderPaymentPending = "Payment Pending";
        public const string OrderReceived = "Received";
        public const string OrderProcessed = "Processed";
        public const string OrderShipped = "Shipped";
        public const string OrderOutForDelivery = "Out For Delivery";
        public const string OrderDelivered = "Delivered";
        public const string OrderCancelled = "Cancelled";
        public const string OrderReturned = "Returned";

        public const string OrderSelfPickupPending = "Pending";
        public const string OrderSelfPickupReady = "Ready for Pickup";
        public const string OrderSelfPickupPicked = "Picked Up";
    }
}
