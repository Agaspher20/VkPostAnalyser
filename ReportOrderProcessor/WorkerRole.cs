using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Threading;
using VkPostAnalyser.Domain.Configuration;
using VkPostAnalyser.Domain.Model;
using VkPostAnalyser.Domain.Services;
using VkPostAnalyser.Domain.Services.VkApi;

namespace ReportOrderProcessor
{
    public class WorkerRole : RoleEntryPoint
    {
        private QueueClient _reportsQueueClient;
        private ManualResetEvent _completedEvent = new ManualResetEvent(false);

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
                        var reportOrder = receivedMessage.GetBody<ServiceBusReportOrder>();
                        var reportBuilder = InitReportBuilder();
                        reportBuilder.BuildReportAsync(reportOrder).Wait();
                        receivedMessage.Complete();
                    }
                    catch(Exception exc)
                    {
                        Trace.TraceError("Exception has bee thrown on reading posts.\n"
                            + "Exception type: {0}\n"
                            + "Exception message: {1}\n"
                            + "Stack trace: {2}", exc.GetType(), exc.Message, exc.StackTrace);
                    }
                });

            _completedEvent.WaitOne();
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
            _completedEvent.Set();
            base.OnStop();
        }

        private IReportBuilder InitReportBuilder()
        {
            return new ReportBuilder(new DataContext(), new VkApiProvider());
        }

        private IReportsQueueConnector BuildReportsQueueConnector()
        {
            var serviceBusConfig = (ServiceBusConfiguration)ConfigurationManager.GetSection(ServiceBusConfiguration.SectionName);
            return new ReportsQueueConnector(serviceBusConfig);
        }
    }
}
