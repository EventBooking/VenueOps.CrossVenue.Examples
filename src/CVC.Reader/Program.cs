// See https://aka.ms/new-console-template for more information

using CVC.Reader;

var dir = Environment.GetEnvironmentVariable("CvcDir");
if (string.IsNullOrWhiteSpace(dir))
    dir = Directory.GetCurrentDirectory();

var clusterCode = Environment.GetEnvironmentVariable("CvcCluster"); // Example: INDV
var tenantId = Environment.GetEnvironmentVariable("CvcTenantId"); // Example: account-3420-A
var accessKey = Environment.GetEnvironmentVariable("CvcAwsAccessKey");
var secretKey = Environment.GetEnvironmentVariable("CvcAwsSecretKey");
var bucketName = Environment.GetEnvironmentVariable("CvcAwsBucket");
var region = Environment.GetEnvironmentVariable("CvcRegion");

var downloader = new SampleDownloader(bucketName, accessKey, secretKey, region);
var prefix = $"{clusterCode.ToLowerInvariant()}/{tenantId}";
await downloader.Download(prefix, dir);
