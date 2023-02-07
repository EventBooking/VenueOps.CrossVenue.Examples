namespace CVC.Common.Helpers;

public static class PayloadHelper
{
    public static string GetPrefix(string clusterCode, string tenantId)
    {
        if (string.IsNullOrWhiteSpace(clusterCode))
            throw new ArgumentException("cannot be null", nameof(clusterCode));
        if (string.IsNullOrWhiteSpace(tenantId))
            throw new ArgumentException("cannot be null", nameof(tenantId));

        return $"{clusterCode.Trim().ToLowerInvariant()}/{tenantId.Trim()}/bulk";
    }
}