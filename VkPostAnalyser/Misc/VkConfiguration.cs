using System.Configuration;
using VKSharp.Data.Api;

namespace VkPostAnalyser.Misc
{
    public class VkConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("applicationId", IsRequired = true)]
        public int ApplicationId
        {
            get
            {
                return (int)this["applicationId"]; 
            }
        }

        [ConfigurationProperty("applicationKey", IsRequired = true)]
        public string ApplicationKey
        {
            get
            {
                return (string)this["applicationKey"];
            }
        }

        [ConfigurationProperty("applicationPermissions", IsRequired = false)]
        public VKPermission ApplicationPermissions
        {
            get
            {
                return (VKPermission)this["applicationPermissions"];
            }
        }
    }
}
