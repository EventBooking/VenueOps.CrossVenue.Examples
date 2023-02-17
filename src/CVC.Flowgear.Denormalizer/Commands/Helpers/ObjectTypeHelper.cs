namespace CVC.Flowgear.Denormalizer.Commands.Helpers
{
    public static class DocumentCollections
    {
        public const string Account = "account";
        public const string Contact = "contact";
        public const string Event = "Event";
        public const string EventDetail = "eventdetail";
        public const string Booking = "booking";
        public const string LiveDetail = "livedetail";
    }

    public static class ChangeBrokerMessageType
    {
        public const string ObjectUpsert = "ObjectUpsert";
        public const string ObjectDelete = "ObjectDelete";
    }
 
    public class MessageBaseModel
    {
        public string Type { get; set; } // ex: ObjectUpsert
    }
}