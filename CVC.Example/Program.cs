using CVC.Example.Services;
using VenueOps.OpenApi;

var dir = Environment.GetEnvironmentVariable("CvcDir");
if (string.IsNullOrWhiteSpace(dir))
    dir = Directory.GetCurrentDirectory();

var clusterCode = Environment.GetEnvironmentVariable("CvcCluster");
var tenantId = Environment.GetEnvironmentVariable("CvcTenantId");
var clientId = Environment.GetEnvironmentVariable("CvcClientId");
var clientSecret = Environment.GetEnvironmentVariable("CvcClientSecret");
        
var client = new ApiClient(clusterCode, clientId, clientSecret);
var generator = new SampleGenerator(client);
var writer = new SampleWriter(dir);

var venueModel = await generator.LoadVenue(clusterCode, tenantId);
await writer.Write(venueModel);

var roomModel = await generator.LoadRoom(clusterCode, tenantId, venueModel.Venue);
await writer.Write(roomModel);

var eventModel = await generator.LoadEvent(clusterCode, tenantId, venueModel);
await writer.Write(eventModel);


