using CVC.Example.Services;
using VenueOps.OpenApi;

var dir = Environment.GetEnvironmentVariable("CvcDir");
if (string.IsNullOrWhiteSpace(dir))
    dir = Directory.GetCurrentDirectory();

var clusterCode = Environment.GetEnvironmentVariable("CvcCluster"); // Example: INDV
var tenantId = Environment.GetEnvironmentVariable("CvcTenantId"); // Example: account-3420-A
var clientId = Environment.GetEnvironmentVariable("CvcClientId"); // VenueOps Open API credentials
var clientSecret = Environment.GetEnvironmentVariable("CvcClientSecret"); // VenueOps Open API credentials
var client = new ApiClient(clusterCode, clientId, clientSecret);

// INCREMENTAL
var incrementalGenerator = new IncrementalSampleGenerator(client);
var incrementalWriter = new SampleWriter(Path.Join(dir, "incremental"));

var venueModel = await incrementalGenerator.LoadVenue(clusterCode, tenantId);
await incrementalWriter.Write(venueModel);

var roomModel = await incrementalGenerator.LoadRoom(clusterCode, tenantId, venueModel.Venue);
await incrementalWriter.Write(roomModel);

var eventModel = await incrementalGenerator.LoadEvent(clusterCode, tenantId, venueModel);
await incrementalWriter.Write(eventModel);

// BULK
var bulkGenerator = new BulkSampleGenerator(client);
var bulkWriter = new SampleWriter(Path.Join(dir, "bulk"));

var setupModel = await bulkGenerator.LoadSetup(clusterCode, tenantId);
await bulkWriter.Write(setupModel);

const int pageSize = 28;
var start = DateTime.Parse("2022-12-01");
    
var roomIds = venueModel.Rooms.Select(x => x.Id).ToList();
for (var page = 0; page < 4; page++)
{
    var bulkEvents = await bulkGenerator.LoadEvents(clusterCode, tenantId, venueModel.Venue.Id, roomIds, start, start.AddDays(pageSize));
    await bulkWriter.Write(bulkEvents, page);
    start += TimeSpan.FromDays(28);
}
