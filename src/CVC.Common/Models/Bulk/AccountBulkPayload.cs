using VenueOps.OpenApi.Models;

namespace CVC.Common.Models.Bulk;

public class AccountBulkPayload : BasePayload
{
    public IEnumerable<AccountResponse> Accounts { get; set; }
}
