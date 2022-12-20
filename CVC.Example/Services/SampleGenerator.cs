using CVC.Example.Models;
using VenueOps.OpenApi;

namespace CVC.Example.Services;

public class SampleGenerator
{
    private readonly ApiClient _client;

    public SampleGenerator(ApiClient client)
    {
        _client = client;
    }

    public async Task<VenueChangePayload> LoadVenue(string clusterCode, string tenantId)
    {
        var allVenues = await _client.GeneralSetup.GetVenuesAsync();
        var allRooms = await _client.GeneralSetup.GetRoomsAsync();
        
        var venue = allVenues.Response.First();
        var rooms = allRooms.Response.Where(x => x.VenueId == venue.Id);

        var model = new VenueChangePayload
        {
            ClusterCode = clusterCode,
            TenantId = tenantId,
            Venue = venue,
            Rooms = rooms
        };
        return model;
    }

    public async Task<RoomChangePayload> LoadRoom(string clusterCode, string tenantId, VenueResponse venue)
    {
        var allRooms = await _client.GeneralSetup.GetRoomsAsync();
        
        var room = allRooms.Response.First(x => x.VenueId == venue.Id);

        var model = new RoomChangePayload
        {
            ClusterCode = clusterCode,
            TenantId = tenantId,
            Venue = venue,
            Room = room
        };
        return model;
    }

    public async Task<EventChangePayload> LoadEvent(string clusterCode, string tenantId, VenueChangePayload venue)
    {
        var allVenues = await _client.GeneralSetup.GetVenuesAsync();
        var allRooms = await _client.GeneralSetup.GetRoomsAsync();
        
        // var request = new QueryEventsRequest
        // {
        //     VenueIds = new List<string>{venue.Venue.Id},
        //     ShowActiveOnly = true,
        //     SearchText = "Invoice"
        // };
        // var events = await _client.Events.QueryEventsAsync(request);
        var request2 = new QueryEventsByDateRangeRequest
        {
            VenueIds = new List<string>{venue.Venue.Id},
            RoomIds = venue.Rooms.Select(x=>x.Id),
            Start = "2022-12-01",
            End = "2023-01-01"
        };
        var events2 = await _client.Events.QueryByDateRangeAsync(request2);
        var oneEvent = events2.Response.First();

        var bookedSpaces = await _client.BookedSpaces.GetBookedSpacesAsync(oneEvent.Id);
        var venues = allVenues.Response.Where(x => oneEvent.VenueIds.Contains(x.Id));
        var rooms = allRooms.Response.Where(x => oneEvent.RoomIds.Contains(x.Id));
        
        var model = new EventChangePayload
        {
            ClusterCode = clusterCode,
            TenantId = tenantId,
            Event = oneEvent,
            BookedSpaces = bookedSpaces.Response,
            Venues = venues,
            Rooms = rooms
        };
        return model;
    }
}
