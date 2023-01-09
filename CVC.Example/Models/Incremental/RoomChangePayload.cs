namespace CVC.Example.Models.Incremental;

public class RoomChangePayload : BasePayload
{
    public VenueResponse Venue { get; set; }
    public RoomResponse Room { get; set; }
}