using VenueOps.OpenApi.Models;

namespace CVC.Common.Models.Bulk;

// TODO: Not yet supported
public class AccountBulkPayload : BasePayload
{
    public IEnumerable<AccountResponse> Accounts { get; set; }
}
