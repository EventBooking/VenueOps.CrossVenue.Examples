using VenueOps.OpenApi.Models;

namespace CVC.Common.Models.Bulk;

public class SetupBulkPayload : BasePayload
{
    public IEnumerable<VenueBulkSingle> Venues { get; set; }
    public IEnumerable<EventTypeResponse> EventTypes { get; set; }
    public IEnumerable<BusinessClassificationResponse> BusinessClassifications { get; set; }
    public IEnumerable<SpaceUsageResponse> SpaceUsages { get; set; }
}

public class VenueBulkSingle
{
    public VenueResponse Venue { get; set; }
    public IEnumerable<RoomResponse> Rooms { get; set; }
}