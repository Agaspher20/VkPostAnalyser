using Microsoft.Owin.Security;
using System.Collections.Generic;

namespace VkPostAnalyser.Model
{
    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }

        public ICollection<AuthenticationDescription> LoginProviders { get; set; }
    }
}
