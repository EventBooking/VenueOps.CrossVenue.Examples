namespace CVC.Common.Models;

public class IncrementalBasePayload : BasePayload
{
    public string Operation { get; set; }
    public string ObjectType { get; set; }
    public string ObjectId { get; set; }
}