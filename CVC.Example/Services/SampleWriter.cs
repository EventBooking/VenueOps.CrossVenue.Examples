using Newtonsoft.Json;

namespace CVC.Example.Services;

public class SampleWriter
{
    private readonly string _dir;

    public SampleWriter(string dir)
    {
        _dir = dir;
    }

    public async Task Write<T>(T obj)
    {
        var fileName = typeof(T).Name + ".json";
        var path = Path.Combine(_dir, fileName);
        var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
        await File.WriteAllTextAsync(path, json);
    }
}