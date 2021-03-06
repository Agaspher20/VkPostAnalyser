﻿using System.Configuration;
using VKSharp.Data.Api;

namespace VkPostAnalyser.Domain.Configuration
{
    public class VkConfiguration : ConfigurationSection
    {
        public const string SectionName = "vkConfiguration";

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
