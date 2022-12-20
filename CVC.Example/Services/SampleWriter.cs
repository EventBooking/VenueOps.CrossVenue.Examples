using CVC.Example.Models;
using Newtonsoft.Json;

namespace CVC.Example.Services;

public class SampleWriter
{
    private readonly string _dir;

    public SampleWriter(string dir)
    {
        _dir = dir;
    }

    public async Task Write<T>(T obj) where T: BasePayload
    {
        if (string.IsNullOrWhiteSpace(obj.ClusterCode))
            throw new Exception("Must include ClusterCode");
        if (string.IsNullOrWhiteSpace(obj.TenantId))
            throw new Exception("Must include TenantId");
        
        var fileName = typeof(T).Name + ".json";
        var path = Path.Combine(_dir, fileName);
        var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
        await File.WriteAllTextAsync(path, json);
    }
}