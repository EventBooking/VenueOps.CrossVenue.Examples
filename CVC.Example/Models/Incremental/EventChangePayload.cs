namespace CVC.Example.Models.Incremental;

public class EventChangePayload : BasePayload
{
    public EventResponse Event { get; set; }
    public IEnumerable<BookedSpaceResponse> BookedSpaces { get; set; }
    public IEnumerable<VenueResponse> Venues { get; set; }
    public IEnumerable<RoomResponse> Rooms { get; set; }
}