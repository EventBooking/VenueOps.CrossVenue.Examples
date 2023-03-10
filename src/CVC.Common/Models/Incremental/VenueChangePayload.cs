using VenueOps.OpenApi.Models;

namespace CVC.Common.Models.Incremental;

public class VenueChangePayload : IncrementalBasePayload
{
    public VenueResponse Venue { get; set; }
    public IEnumerable<RoomResponse> Rooms { get; set; }
}