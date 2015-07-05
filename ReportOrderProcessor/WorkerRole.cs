using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Threading;
using VkPostAnalyser.Domain.Configuration;
using VkPostAnalyser.Domain.Services;

namespace ReportOrderProcessor
{
    public class WorkerRole : RoleEntryPoint
    {
        private QueueClient _reportsQueueClient;

        ManualResetEvent CompletedEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.WriteLine("Starting processing of messages");

            // Initiates the message pump and callback is invoked for each message that is received, calling close on the client will stop the pump.
            _reportsQueueClient.OnMessage((receivedMessage) =>
                {
                    try
                    {
                        // Process the message
                        Trace.WriteLine("Processing Service Bus message: " + receivedMessage.SequenceNumber.ToString());
                    }
                    catch
                    {
                        // Handle any message processing specific exceptions here
                    }
                });

            CompletedEvent.WaitOne();
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;
            var reportsQueueConnector = BuildReportsQueueConnector();
            _reportsQueueClient = reportsQueueConnector.ReportsQueueClient;
            return base.OnStart();
        }

        public override void OnStop()
        {
            // Close the connection to Service Bus Queue
            _reportsQueueClient.Close();
            CompletedEvent.Set();
            base.OnStop();
        }

        private IReportsQueueConnector BuildReportsQueueConnector()
        {
            var serviceBusConfig = (ServiceBusConfiguration)ConfigurationManager.GetSection(ServiceBusConfiguration.SectionName);
            return new ReportsQueueConnector(serviceBusConfig);
        }
    }
}
