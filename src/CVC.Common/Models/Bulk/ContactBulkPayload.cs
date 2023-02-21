using VenueOps.OpenApi.Models;

namespace CVC.Common.Models.Bulk;

// TODO: Not yet supported
public class ContactBulkPayload : BasePayload
{
    public IEnumerable<ContactResponse> Contacts { get; set; }
}
