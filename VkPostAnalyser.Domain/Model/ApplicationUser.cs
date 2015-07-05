using Microsoft.AspNet.Identity;

namespace VkPostAnalyser.Domain.Model
{
    public class ApplicationUser : IUser<int>
    {
        public int Id { get; set; }

        public string Token { get; set; }

        public string UserName { get; set; }

        public string UserAlias { get; set; }

        public string LoginType { get; set; }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
