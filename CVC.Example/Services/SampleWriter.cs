using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using CVC.Example.Models;
using Newtonsoft.Json;

namespace CVC.Example.Services;

public class SampleWriter
{
    private readonly string _dir;
    private readonly string _bucket;
    private readonly string _prefix;

    public SampleWriter(string dir, string bucket, string prefix)
    {
        _dir = dir;
        _bucket = bucket;
        _prefix = prefix;
        if (!Directory.Exists(_dir))
            throw new Exception($"Please create the {_dir} directory before running");
    }

    public async Task Write<T>(T obj, int? page = null) where T : BasePayload
    {
        if (string.IsNullOrWhiteSpace(obj.ClusterCode))
            throw new Exception("Must include ClusterCode");
        if (string.IsNullOrWhiteSpace(obj.TenantId))
            throw new Exception("Must include TenantId");

        var fileName = page.HasValue
            ? typeof(T).Name + $"-{page.Value}.json"
            : typeof(T).Name + ".json";
        var path = Path.Combine(_dir, fileName);
        var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
        await File.WriteAllTextAsync(path, json);

        if (string.IsNullOrWhiteSpace(_bucket) || string.IsNullOrWhiteSpace(_prefix))
            return;

        var key = $"{_prefix}/{fileName}";
        var client = new AmazonS3Client();
        var req = new PutObjectRequest
        {
            BucketName = _bucket,
            Key = key,
            FilePath = path,
            CannedACL = S3CannedACL.BucketOwnerFullControl
        };
        var response = await client.PutObjectAsync(req);
        if (response.HttpStatusCode != HttpStatusCode.OK)
            throw new Exception($"Failure! HTTP Code = {response.HttpStatusCode}");
    }
}