using System;
using CVC.Flowgear.Denormalizer.Commands.Helpers;
using CVC.Flowgear.Denormalizer.Config;
using CVC.Flowgear.Denormalizer.Helpers;
using CVC.Flowgear.Denormalizer.Services;
using CVC.Flowgear.Denormalizer.Services.Handlers;
using flowgear.Sdk;
using Newtonsoft.Json;
using VenueOps.OpenApi.BaseClasses;
using VenueOps.OpenApi.Internal;

namespace CVC.Flowgear.Denormalizer
{
    [Node("CvcDenormalizer", "CVC Denormalizer", NodeType.Connector, "icon.png", RunFrom.Anywhere)]
    public class CvcDenormalizer
    {
        [Property(FlowDirection.Input, ExtendedType.ConnectionProfile)]
        public CvcDenormalizerConnection Connection { get; set; }

        [Property(FlowDirection.Input, ExtendedType.Json)]
        public string OpenApiCredentials { get; set; }

        [Property(FlowDirection.Input, ExtendedType.Json)]
        public string RequestJson { get; set; }

        [Property(FlowDirection.Output, ExtendedType.None)]
        public string ObjectType { get; set; }

        [Property(FlowDirection.Output, ExtendedType.Json)]
        public string ResponseJson { get; set; }

        [Property(FlowDirection.Output, ExtendedType.MultilineText)]
        public string OpenApiAuthToken { get; set; }

        [Property(FlowDirection.Input, ExtendedType.None)]
        public bool UseExceptionResult { get; set; }

        [Property(FlowDirection.Output, ExtendedType.MultilineText)]
        public string Logs { get; set; }

        [Invoke]
        [InvokeResult("Upsert", false)]
        [InvokeResult("Delete", false)]
        [InvokeResult("Exception", false)]
        [InvokeResult("Noop", true)]
        public InvokeResult Invoke()
        {
            var logger = new SimpleLogger();
            var apiClient = GetClient();
            var flattener = new EventFlattener(apiClient, logger);

            var model = JsonConvert.DeserializeObject<MessageBaseModel>(RequestJson);
            var clusterCode = GetClusterIdentifier().ToString();

            try
            {
                switch (model.Type)
                {
                    case ChangeBrokerMessageType.ObjectUpsert:
                        var upsertHandler = new QueueUpsertHandler(apiClient, flattener, logger);
                        var upsertResult = upsertHandler.Execute(clusterCode, RequestJson).Result;
                        if (upsertResult != null)
                        {
                            ObjectType = upsertResult.ObjectType;
                            ResponseJson = JsonConvert.SerializeObject(upsertResult);
                            OpenApiAuthToken = apiClient.AccessToken;
                            Logs = logger.Read();
                            return new InvokeResult("Upsert");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                var message = ExceptionHelper.Render(ex, logger.Read());
                if (!UseExceptionResult)
                    throw new Exception(message);
                ObjectType = "none";
                ResponseJson = string.Empty;
                OpenApiAuthToken = apiClient.AccessToken;
                Logs = message;
                return new InvokeResult("Exception");
            }

            ObjectType = "none";
            ResponseJson = string.Empty;
            OpenApiAuthToken = apiClient.AccessToken;
            Logs = logger.Read();
            return new InvokeResult("Noop");
        }

        private IInternalApiClient GetClient()
        {
            var clusterId = GetClusterIdentifier();
            var credentials = JsonConvert.DeserializeObject<OpenApiCredentials>( OpenApiCredentials );
            return new InternalApiClient(clusterId, credentials.ClientId, credentials.ClientSecret);
        }

        private VenueOpsCluster GetClusterIdentifier()
        {
            switch (Connection.Cluster)
            {
                // case Cluster.Manual:
                //     var cluster = new ClusterInfo
                //     {
                //         AuthenticationUrl = Connection.AuthUrl,
                //         ApiUrl = Connection.ApiUrl
                //     };
                //     return new InternalApiClient(cluster, Connection.ClientId, Connection.ClientSecret);

                case Cluster.Indv:
                    return VenueOpsCluster.Indv;

                case Cluster.ProductionNorthAmerica:
                    return VenueOpsCluster.NorthAmerica;

                case Cluster.ProdCanada:
                    return VenueOpsCluster.Canada;

                case Cluster.ProdEMEA:
                    return VenueOpsCluster.Europe;

                case Cluster.ProdAPAC:
                    return VenueOpsCluster.AsiaPacific;

                case Cluster.Staging:
                default:
                    throw new ArgumentOutOfRangeException();
            }            
        }
    }

    internal class OpenApiCredentials
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}