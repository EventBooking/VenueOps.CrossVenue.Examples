using VenueOps.OpenApi.Models;

namespace CVC.Common.Models.Bulk;

public class ContactBulkPayload : BasePayload
{
    public IEnumerable<ContactResponse> Contacts { get; set; }
}
