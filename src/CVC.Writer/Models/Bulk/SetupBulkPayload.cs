namespace CVC.Example.Models.Bulk;

public class SetupBulkPayload : BasePayload
{
    public IEnumerable<VenueBulkSingle> Venues { get; set; }
}

public class VenueBulkSingle
{
    public VenueResponse Venue { get; set; }
    public IEnumerable<RoomResponse> Rooms { get; set; }
}