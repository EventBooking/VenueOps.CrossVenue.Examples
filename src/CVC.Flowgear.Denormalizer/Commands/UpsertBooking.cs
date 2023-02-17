using System.Threading.Tasks;
using CVC.Common.Models;
using CVC.Flowgear.Denormalizer.Commands.Models;
using CVC.Flowgear.Denormalizer.Services;
using VenueOps.OpenApi.Internal;

namespace CVC.Flowgear.Denormalizer.Commands
{
    public class UpsertBooking : IUpsertCommand
    {
        private readonly IInternalApiClient _api;
        private readonly IEventFlattener _flattener;
        private readonly ISimpleLogger _logger;

        public UpsertBooking(IInternalApiClient api, IEventFlattener flattener, ISimpleLogger logger)
        {
            _api = api;
            _flattener = flattener;
            _logger = logger;
        }

        public async Task<IncrementalBasePayload> Execute(string clusterCode, string tenantId, string documentId)
        {
            var eventResponse = await _api.Internal.GetEventIdFromBookingIdAsync(documentId);
            if (eventResponse.IsSuccessStatusCode)
                return await _flattener.UpsertEvent(clusterCode, tenantId, eventResponse.Response.Id);

            return null;
        }
    }
}