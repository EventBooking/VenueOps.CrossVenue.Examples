using VenueOps.OpenApi.Models;

namespace CVC.Common.Models.Incremental;

public class RoomChangePayload : IncrementalBasePayload
{
    public VenueResponse Venue { get; set; }
    public RoomResponse Room { get; set; }
}