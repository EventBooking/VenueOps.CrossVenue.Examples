using System.Threading.Tasks;
using CVC.Common.Models;
using CVC.Flowgear.Denormalizer.Commands;
using CVC.Flowgear.Denormalizer.Commands.Helpers;
using Newtonsoft.Json;
using VenueOps.OpenApi.Internal;

namespace CVC.Flowgear.Denormalizer.Services.Handlers
{
    public class QueueUpsertHandler
    {
        private readonly IInternalApiClient _api;
        private readonly IEventFlattener _flattener;
        private readonly ISimpleLogger _logger;

        public QueueUpsertHandler(IInternalApiClient api, IEventFlattener flattener, ISimpleLogger logger)
        {
            _api = api;
            _flattener = flattener;
            _logger = logger;
        }

        public async Task<IncrementalBasePayload> Execute(string clusterCode, string json)
        {
            var change = JsonConvert.DeserializeObject<ChangeBrokerModel>(json);

            IUpsertCommand command;
            switch (change.CollectionName)
            {
                case DocumentCollections.Account:
                    command = new UpsertAccount(_api, _logger);
                    break;

                case DocumentCollections.Contact:
                    command = new UpsertContact(_api, _logger);
                    break;
                
                case DocumentCollections.Event:
                    command = new UpsertEvent(_api, _flattener, _logger);
                    break;

                case DocumentCollections.EventDetail:
                    command = new UpsertEventDetail(_api, _flattener, _logger);
                    break;

                case DocumentCollections.Booking:
                    command = new UpsertBooking(_api, _flattener, _logger);
                    break;

                case DocumentCollections.LiveDetail:
                    command = new UpsertLiveEntertainment(_api, _flattener, _logger);
                    break;
               
                default:
                    return new IncrementalBasePayload();
            }

            return await command.Execute(clusterCode, change.TenantId, change.DocumentId);
        }
        
        public class ChangeBrokerModel : MessageBaseModel
        {
            public string TenantId { get; set; } // ex: account-1-A
            public string CollectionName { get; set; } // ex: Event
            public string DocumentId { get; set; } // ex: event-5982-A
        }
    }
}