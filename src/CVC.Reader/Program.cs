using CVC.Common.Models.Bulk;
using CVC.Reader.Services;

var clusterCode = Environment.GetEnvironmentVariable("CvcCluster"); // Example: INDV
var tenantId = Environment.GetEnvironmentVariable("CvcTenantId"); // Example: account-3420-A
var accessKey = Environment.GetEnvironmentVariable("CvcAwsAccessKey");
var secretKey = Environment.GetEnvironmentVariable("CvcAwsSecretKey");
var bucketName = Environment.GetEnvironmentVariable("CvcAwsBucket");
var region = Environment.GetEnvironmentVariable("CvcRegion");

// Download raw files
var dir = Environment.GetEnvironmentVariable("CvcDir");
if (!string.IsNullOrWhiteSpace(dir))
{
    var downloader = new SampleDownloader(bucketName, accessKey, secretKey, region);
    await downloader.Download(clusterCode, tenantId, dir);
}

// Retrieve/deserialize directly
ISampleRetriever retrieve = new SampleRetriever(bucketName, accessKey, secretKey, region);

var setup = await retrieve.RetrieveSetup(clusterCode, tenantId);
Console.WriteLine("Venues");
foreach (var venue in setup.Venues)
{
    Console.WriteLine($"- {venue.Venue.Name}");
}

Console.WriteLine(string.Empty);
Console.WriteLine("Event Types");
foreach (var etype in setup.EventTypes)
{
    Console.WriteLine($"- {etype.Name}");
}

Console.WriteLine(string.Empty);
Console.WriteLine("Business Classifications");
foreach (var bclass in setup.BusinessClassifications)
{
    Console.WriteLine($"- {bclass.Name}");
}

Console.WriteLine(string.Empty);
Console.WriteLine("Events");
var countEventPages = await retrieve.GetPageCount<EventBulkPayload>(clusterCode, tenantId);
for (var page = 0; page < countEventPages; page++)
{
    Console.WriteLine($"- Page {page}");
    var events = await retrieve.Retrieve<EventBulkPayload>(clusterCode, tenantId, page);
    foreach (var evt in events.Events)
    {
        Console.WriteLine($"  - {evt.Event.Name}");
    }
}