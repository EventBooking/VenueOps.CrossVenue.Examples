using System.Net;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using CVC.Common.Helpers;

namespace CVC.Reader.Services;

public class SampleDownloader
{
    private readonly string _bucket;
    private readonly string _accessKey;
    private readonly string _secretKey;
    private readonly RegionEndpoint _region;

    public SampleDownloader(string bucket, string accessKey, string secretKey, string region)
    {
        _bucket = bucket;
        _accessKey = accessKey;
        _secretKey = secretKey;
        _region = RegionEndpoint.GetBySystemName(region);
    }

    public async Task Download(string clusterCode, string tenantId, string dir)
    {
        var cred = new BasicAWSCredentials(_accessKey, _secretKey);
        var client = new AmazonS3Client(cred, _region);

        var request = new ListObjectsRequest
        {
            BucketName = _bucket,
            Prefix = PayloadHelper.GetPrefix(clusterCode, tenantId),
        };
        for (;;)
        {
            var response = await client.ListObjectsAsync(request);

            foreach (var obj in response.S3Objects)
                await DownloadObject(client, obj, dir);

            if (!response.IsTruncated)
                break;
            request.Marker = response.NextMarker;
        }
    }

    private static async Task DownloadObject(IAmazonS3 client, S3Object obj, string dir)
    {
        var request = new GetObjectRequest
        {
            BucketName = obj.BucketName,
            Key = obj.Key
        };
        var response = await client.GetObjectAsync(request);
        if (response.HttpStatusCode == HttpStatusCode.OK)
        {
            var path = Path.Combine(dir, obj.Key);
            await response.WriteResponseStreamToFileAsync(path, false, CancellationToken.None);
        }
    }
}