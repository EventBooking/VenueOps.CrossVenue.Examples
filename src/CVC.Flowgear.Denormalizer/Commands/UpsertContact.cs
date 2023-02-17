using System.Threading.Tasks;
using CVC.Common.Models;
using CVC.Common.Models.Incremental;
using CVC.Flowgear.Denormalizer.Services;
using VenueOps.OpenApi.Internal;

namespace CVC.Flowgear.Denormalizer.Commands
{
    public class UpsertContact : IUpsertCommand
    {
        private readonly IInternalApiClient _api;
        private readonly ISimpleLogger _logger;

        public UpsertContact(IInternalApiClient api, ISimpleLogger logger)
        {
            _api = api;
            _logger = logger;
        }

        public async Task<IncrementalBasePayload> Execute(string clusterCode, string tenantId, string documentId)
        {
            var response = await _api.Contacts.GetContactAsync(documentId);
            if (!response.IsSuccessStatusCode) return null;
            
            var result = new ContactChangePayload
            {
                ClusterCode = clusterCode,
                TenantId = tenantId,
                Operation = ObjectOperation.Upsert,
                ObjectId = documentId,
                ObjectType = ObjectType.Contact,
                Contact = response.Response,
            };
            return result;
        }
    }
}