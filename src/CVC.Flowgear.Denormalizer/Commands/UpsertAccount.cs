using System.Collections.Generic;
using System.Threading.Tasks;
using CVC.Common.Models;
using CVC.Common.Models.Incremental;
using CVC.Flowgear.Denormalizer.Commands.Helpers;
using CVC.Flowgear.Denormalizer.Commands.Models;
using CVC.Flowgear.Denormalizer.Config;
using CVC.Flowgear.Denormalizer.Services;
using Newtonsoft.Json;
using VenueOps.OpenApi.Internal;
using VenueOps.OpenApi.Models;

namespace CVC.Flowgear.Denormalizer.Commands
{
    public class UpsertAccount : IUpsertCommand
    {
        private readonly IInternalApiClient _api;
        private readonly ISimpleLogger _logger;

        public UpsertAccount(IInternalApiClient api, ISimpleLogger logger)
        {
            _api = api;
            _logger = logger;
        }

        public async Task<IncrementalBasePayload> Execute(string clusterCode, string tenantId, string documentId)
        {
            var response = await _api.Accounts.GetAccountAsync(documentId);
            if (!response.IsSuccessStatusCode) return null;
            
            var result = new AccountChangePayload
            {
                ClusterCode = clusterCode,
                TenantId = tenantId,
                Operation = ObjectOperation.Upsert,
                ObjectId = documentId,
                ObjectType = ObjectType.Account,
                Account = response.Response,
            };
            return result;

        }
    }
}
