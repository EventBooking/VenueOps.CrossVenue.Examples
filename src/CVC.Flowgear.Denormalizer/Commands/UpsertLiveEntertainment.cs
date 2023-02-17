using System.Threading.Tasks;
using CVC.Common.Models;
using CVC.Flowgear.Denormalizer.Services;
using VenueOps.OpenApi.Internal;

namespace CVC.Flowgear.Denormalizer.Commands
{
    public class UpsertLiveEntertainment : IUpsertCommand
    {
        private readonly IInternalApiClient _api;
        private readonly IEventFlattener _flattener;
        private readonly ISimpleLogger _logger;

        public UpsertLiveEntertainment(IInternalApiClient api, IEventFlattener flattener, ISimpleLogger logger)
        {
            _api = api;
            _flattener = flattener;
            _logger = logger;
        }

        public async Task<IncrementalBasePayload> Execute(string clusterCode, string tenantId, string documentId)
        {
            var eventIdResponse = await _api.Internal.GetEventIdFromLiveDetailIdAsync(documentId);
            if (eventIdResponse.IsSuccessStatusCode)
                return await _flattener.UpsertEvent(clusterCode, tenantId, eventIdResponse.Response.Id);

            return null;
        }
    }
}