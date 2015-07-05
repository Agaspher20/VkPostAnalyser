using System.Configuration;

namespace VkPostAnalyser.Domain.Configuration
{
    public class ServiceBusConfiguration : ConfigurationSection
    {
        public const string SectionName = "serviceBusCredentials";

        [ConfigurationProperty("queueName", IsRequired = true)]
        public string QueueName
        {
            get
            {
                return (string)this["queueName"];
            }
        }

        [ConfigurationProperty("namespace", IsRequired = true)]
        public string Namespace
        {
            get
            {
                return (string)this["namespace"];
            }
        }

        [ConfigurationProperty("policyName", IsRequired = true)]
        public string PolicyName
        {
            get
            {
                return (string)this["policyName"];
            }
        }

        [ConfigurationProperty("primaryKey", IsRequired = true)]
        public string PrimaryKey
        {
            get
            {
                return (string)this["primaryKey"];
            }
        }
    }
}
