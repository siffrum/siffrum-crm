namespace Siffrum.Ecom.DomainModels.Enums
{
    public enum DeliveryBoyStatusDM
    {
        Registered = 0,
        Active = 1,
        Rejected = 2,
        Deactivated = 3,
        Removed = 7
    }

    public static class DeliveryBoyStatusText
    {
        public static string Registered = "Registered";
        public static string Active = "Active";
        public static string Rejected = "Rejected";
        public static string Deactivated = "Deactivated";
        public static string Removed = "Removed";
    }
}
