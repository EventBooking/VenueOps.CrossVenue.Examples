using flowgear.Sdk;

namespace CVC.Flowgear.Denormalizer.Config
{
    public class CvcDenormalizerConnection
    {
        [Property(ExtendedType.None)]
        public Cluster Cluster { get; set; } = Cluster.Indv;
    }
}