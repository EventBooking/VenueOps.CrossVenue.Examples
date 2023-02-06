using CVC.Common.Models.Bulk;
using VenueOps.OpenApi;

namespace CVC.Writer.Services;

public class BulkSampleGenerator
{
    private readonly ApiClient _client;

    public BulkSampleGenerator(ApiClient client)
    {
        _client = client;
    }

    public async Task<SetupBulkPayload> LoadSetup(string clusterCode, string tenantId)
    {
        var allVenues = await _client.GeneralSetup.GetVenuesAsync();
        var allRooms = await _client.GeneralSetup.GetRoomsAsync();

        var venues = allVenues.Response.Select(v => new VenueBulkSingle
        {
            Venue = v,
            Rooms = allRooms.Response.Where(r => r.VenueId == v.Id)
        });
        
        var model = new SetupBulkPayload
        {
            ClusterCode = clusterCode,
            TenantId = tenantId,
            Venues = venues
        };
        return model;
    }

    public async Task<EventBulkPayload> LoadEvents(string clusterCode, string tenantId, string venueId, IEnumerable<string> roomIds, DateTime start, DateTime end) 
    {
        var request = new QueryEventsByDateRangeRequest
        {
            VenueIds = new List<string>{venueId},
            RoomIds = roomIds,
            Start = $"{start:yyyy-MM-dd}",
            End = $"{end:yyyy-MM-dd}"
        };
        var events = await _client.Events.QueryByDateRangeAsync(request);
       
        var eventModels = new List<EventBulkSingle>();
        foreach (var evt in events.Response)
        {
            var bookedSpaces = await _client.BookedSpaces.GetBookedSpacesAsync(evt.Id);
            var eventModel = new EventBulkSingle
            {
                Event = evt,
                BookedSpaces = bookedSpaces.Response
            };
            eventModels.Add(eventModel);
        }

        var model = new EventBulkPayload
        {
            TenantId = tenantId,
            ClusterCode = clusterCode,
            Events = eventModels
        };
        return model;
    }
}
