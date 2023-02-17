using VenueOps.OpenApi.Models;

namespace CVC.Common.Models.Incremental;

public class EventChangePayload : IncrementalBasePayload
{
    public EventResponse Event { get; set; }
    public IEnumerable<BookedSpaceResponse> BookedSpaces { get; set; }
    public AccountResponse Account { get; set; }
    public IEnumerable<VenueResponse> Venues { get; set; }
    public IEnumerable<RoomResponse> Rooms { get; set; }
}

public class AccountChangePayload : IncrementalBasePayload
{
    public AccountResponse Account { get; set; }
}

public class ContactChangePayload : IncrementalBasePayload
{
    public ContactResponse Contact { get; set; }
}