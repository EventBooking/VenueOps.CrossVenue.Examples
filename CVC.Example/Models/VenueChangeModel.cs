namespace CrossVenue.Examples.CVC.Models;

public class VenueChangeModel
{
    public VenueResponse Venue { get; set; }
    public IEnumerable<RoomResponse> Rooms { get; set; }
}