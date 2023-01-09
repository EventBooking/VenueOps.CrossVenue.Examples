namespace CVC.Example.Models.Bulk;

public class EventBulkPayload : BasePayload
{
    public IEnumerable<EventBulkSingle> Events { get; set; }
}

public class EventBulkSingle
{
    public EventResponse Event { get; set; }
    public IEnumerable<BookedSpaceResponse> BookedSpaces { get; set; }
}