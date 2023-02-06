using CVC.Writer.Services;
using VenueOps.OpenApi;

var dir = Environment.GetEnvironmentVariable("CvcDir");
if (string.IsNullOrWhiteSpace(dir))
    dir = Directory.GetCurrentDirectory();

var clusterCode = Environment.GetEnvironmentVariable("CvcCluster") ?? string.Empty; // Example: INDV
var tenantId = Environment.GetEnvironmentVariable("CvcTenantId") ?? string.Empty; // Example: account-3420-A
var clientId = Environment.GetEnvironmentVariable("CvcClientId"); // VenueOps Open API credentials
var clientSecret = Environment.GetEnvironmentVariable("CvcClientSecret"); // VenueOps Open API credentials
var client = new ApiClient(clusterCode, clientId, clientSecret);

var prefix = $"{clusterCode.ToLowerInvariant()}/{tenantId}";

// INCREMENTAL
var incrementalGenerator = new IncrementalSampleGenerator(client);
var incrementalWriter = new SampleWriter(Path.Join(dir, "incremental"), "cvc.sample.data", $"{prefix}/incremental");

var venueModel = await incrementalGenerator.LoadVenue(clusterCode, tenantId);
await incrementalWriter.Write(venueModel);

var roomModel = await incrementalGenerator.LoadRoom(clusterCode, tenantId, venueModel.Venue);
await incrementalWriter.Write(roomModel);

var eventModel = await incrementalGenerator.LoadEvent(clusterCode, tenantId, venueModel);
await incrementalWriter.Write(eventModel);

// BULK
var bulkGenerator = new BulkSampleGenerator(client);
var bulkWriter = new SampleWriter(Path.Join(dir, "bulk"), "cvc.sample.data", $"{prefix}/bulk");

var setupModel = await bulkGenerator.LoadSetup(clusterCode, tenantId);
await bulkWriter.Write(setupModel);

var start = DateTime.Parse("2022-12-01");
var end = DateTime.Parse("2025-12-01");
var pageSize = TimeSpan.FromDays(28);
    
var roomIds = venueModel.Rooms.Select(x => x.Id).ToList();
var page = 0;
for ( var date = start; date < end; date += pageSize)
{
    var bulkEvents =
        await bulkGenerator.LoadEvents(clusterCode, tenantId, venueModel.Venue.Id, roomIds, date, date + pageSize);
    if (bulkEvents.Events.Any())
    {
        await bulkWriter.Write(bulkEvents, page);
        page++;
    }
}

// Bucket: cvc.sample.data
