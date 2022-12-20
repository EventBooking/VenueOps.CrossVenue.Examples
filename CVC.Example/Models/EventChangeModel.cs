namespace CrossVenue.Examples.CVC.Models;

public class EventChangeModel
{
    public EventResponse Event { get; set; }
    public IEnumerable<BookedSpaceResponse> BookedSpaces { get; set; }
    public IEnumerable<VenueResponse> Venues { get; set; }
    public IEnumerable<RoomResponse> Rooms { get; set; }
}