using CVC.Example.Services;
using VenueOps.OpenApi;

var dir = Environment.GetEnvironmentVariable("CvcDir");
if (string.IsNullOrWhiteSpace(dir))
    dir = Directory.GetCurrentDirectory();

var clusterCode = Environment.GetEnvironmentVariable("CvcCluster");
var clientId = Environment.GetEnvironmentVariable("CvcClientId");
var clientSecret = Environment.GetEnvironmentVariable("CvcClientSecret");
        
var client = new ApiClient(clusterCode, clientId, clientSecret);
var generator = new SampleGenerator(client);
var writer = new SampleWriter(dir);

var venueModel = await generator.LoadVenue();
await writer.Write(venueModel);

var roomModel = await generator.LoadRoom(venueModel.Venue);
await writer.Write(roomModel);

var eventModel = await generator.LoadEvent(venueModel);
await writer.Write(eventModel);


