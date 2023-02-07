using System.Text;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using CVC.Common.Helpers;
using CVC.Common.Models;
using CVC.Common.Models.Bulk;
using Newtonsoft.Json;

namespace CVC.Reader.Services;

public interface ISampleRetriever
{
    Task<SetupBulkPayload> RetrieveSetup(string clusterCode, string tenantId);
    Task<int?> GetPageCount<T>(string clusterCode, string tenantId) where T: BasePayload;
    Task<T> Retrieve<T>(string clusterCode, string tenantId, int? page = null) where T: BasePayload;
}

public class SampleRetriever : ISampleRetriever
{
    private readonly string _bucket;
    private readonly AmazonS3Client _client;

    public SampleRetriever(string bucket, string accessKey, string secretKey, string regionName)
    {
        _bucket = bucket;
        var region = RegionEndpoint.GetBySystemName(regionName);
        var cred = new BasicAWSCredentials(accessKey, secretKey);
        _client = new AmazonS3Client(cred, region);
    }

    public async Task<SetupBulkPayload> RetrieveSetup(string clusterCode, string tenantId)
    {
        var setup = await Retrieve<SetupBulkPayload>(clusterCode, tenantId);
        return setup;
    }

    public async Task<int?> GetPageCount<T>(string clusterCode, string tenantId) where T: BasePayload
    {
        var prefix = PayloadHelper.GetPrefix(clusterCode, tenantId);
        var typeName = typeof(T).Name;
        var count = 0;

        var request = new ListObjectsRequest
        {
            BucketName = _bucket,
            Prefix = $"{prefix}/{typeName}"
        };
        for (;;)
        {
            var response = await _client.ListObjectsAsync(request);
            count += response.S3Objects.Count;
            if (!response.IsTruncated)
                break;
            request.Marker = response.NextMarker;
        }

        return count;
    }
    
    public async Task<T> Retrieve<T>(string clusterCode, string tenantId, int? page = null) where T: BasePayload
    {
        var typeName = typeof(T).Name;
        var prefix = PayloadHelper.GetPrefix(clusterCode, tenantId);
        
        var key = page.HasValue
            ? $"{prefix}/{typeName}-{page}.json"
            : $"{prefix}/{typeName}.json";
        
        var model = await DownloadObject<T>(key);
        return model;
    }

    private async Task<T> DownloadObject<T>(string key )
    {
        var request = new GetObjectRequest
        {
            BucketName = _bucket,
            Key = key
        };
        var response = await _client.GetObjectAsync(request);
        using var reader = new StreamReader(response.ResponseStream, Encoding.UTF8);
        var json = await reader.ReadToEndAsync();
        var model = JsonConvert.DeserializeObject<T>(json);
        return model;
    }
}