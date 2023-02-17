using System.Threading.Tasks;
using CVC.Common.Models.Incremental;
using VenueOps.OpenApi.Internal;

namespace CVC.Flowgear.Denormalizer.Services
{
    public interface IEventFlattener
    {
        Task<EventChangePayload> UpsertEvent(string clusterCode, string tenantId, string eventId);
    }

    public class EventFlattener : IEventFlattener
    {
        private readonly IInternalApiClient _api;
        private readonly ISimpleLogger _logger;

        public EventFlattener(IInternalApiClient api,  ISimpleLogger logger)
        {
            _api = api;
            _logger = logger;
        }

        public async Task<EventChangePayload> UpsertEvent(string clusterCode, string tenantId, string eventId)
        {
            var eventResult = await _api.Events.GetEventAsync(eventId);
            if (!eventResult.IsSuccessStatusCode) 
                return null;
            
            //_logger.Log("Calling GetAccount");
            var accountResponse = await _api.Accounts.GetAccountAsync(eventResult.Response.AccountId);
            var venueResponse = await _api.GeneralSetup.GetVenuesAsync();
            var roomResponse = await _api.GeneralSetup.GetRoomsAsync();
            
            var bookedSpaceResponse = await _api.BookedSpaces.GetBookedSpacesAsync(eventId);
            //var liveResponse = await _api.Events.GetLiveEntertainmentInfoAsync(eventId);

            // var result = new ResponseModel(OutgoingMessageType.QueueUpsert,
            //     InternalObjectType.Event,
            //     JsonConvert.SerializeObject(dataModel));
            var result = new EventChangePayload
            {
                ClusterCode = clusterCode,
                TenantId = tenantId,
                Operation = ObjectOperation.Upsert,
                ObjectType = ObjectType.Event,
                ObjectId = eventId,
                Event = eventResult.Response,
                BookedSpaces = bookedSpaceResponse.IsSuccessStatusCode ? bookedSpaceResponse.Response : null,
                Account = accountResponse.Response,
                Venues = venueResponse.Response,
                Rooms = roomResponse.Response
            };
            return result;
        }
    }
}
