using System.Threading.Tasks;
using CVC.Common.Models;

namespace CVC.Flowgear.Denormalizer.Commands
{
    public interface IUpsertCommand
    {
        Task<IncrementalBasePayload> Execute(string clusterCode, string tenantId, string documentId);
    }
}
