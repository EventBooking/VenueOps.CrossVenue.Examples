namespace CVC.Example.Models.Incremental;

public class VenueChangePayload : BasePayload
{
    public VenueResponse Venue { get; set; }
    public IEnumerable<RoomResponse> Rooms { get; set; }
}