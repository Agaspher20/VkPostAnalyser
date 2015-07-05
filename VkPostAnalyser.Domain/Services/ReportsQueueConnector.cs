using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using VkPostAnalyser.Domain.Configuration;

namespace VkPostAnalyser.Domain.Services
{
    public interface IReportsQueueConnector
    {
        NamespaceManager NamespaceManager { get; }
        QueueClient ReportsQueueClient { get; }
    }

    public class ReportsQueueConnector : IReportsQueueConnector
    {
        private readonly Lazy<string> _connectionStringLazy;
        private readonly Lazy<NamespaceManager> _namespaceManagerLazy;
        private readonly Lazy<QueueClient> _reportsQueueClientLazy;
        private readonly ServiceBusConfiguration _configuration;

        public ReportsQueueConnector(ServiceBusConfiguration configuration)
        {
            _connectionStringLazy = new Lazy<string>(BuildConnectionString);
            _namespaceManagerLazy = new Lazy<NamespaceManager>(BuildNamespaceManager);
            _reportsQueueClientLazy = new Lazy<QueueClient>(InitializeQueueClient);
            _configuration = configuration;
        }

        public string ConnectionString
        {
            get
            {
                return _connectionStringLazy.Value;
            }
        }

        public NamespaceManager NamespaceManager
        {
            get
            {
                return _namespaceManagerLazy.Value;
            }
        }

        public QueueClient ReportsQueueClient
        {
            get
            {
                return _reportsQueueClientLazy.Value;
            }
        }

        private QueueClient InitializeQueueClient()
        {
            // Using Http to be friendly with outbound firewalls
            ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.Http;

            // Create the queue if it does not exist already
            if (!NamespaceManager.QueueExists(_configuration.QueueName))
            {
                NamespaceManager.CreateQueue(_configuration.QueueName);
            }

            // Get a client to the queue
            var messagingFactory = MessagingFactory.Create(NamespaceManager.Address, NamespaceManager.Settings.TokenProvider);
            return messagingFactory.CreateQueueClient(_configuration.QueueName);
        }

        private string BuildConnectionString()
        {
            return "Endpoint=sb://" + _configuration.Namespace + ".servicebus.windows.net/;SharedAccessKeyName=" + _configuration.PolicyName + ";SharedAccessKey=" + _configuration.PrimaryKey;
        }

        private NamespaceManager BuildNamespaceManager()
        {
            var uri = ServiceBusEnvironment.CreateServiceUri("sb", _configuration.Namespace, String.Empty);
            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(_configuration.PolicyName, _configuration.PrimaryKey);
            return new NamespaceManager(uri, tokenProvider);
        }
    }
}
