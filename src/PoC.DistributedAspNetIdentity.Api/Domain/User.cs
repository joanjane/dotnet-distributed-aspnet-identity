using Microsoft.AspNetCore.Identity;

namespace PoC.DistributedAspNetIdentity.Api.Domain
{
    public class User : IdentityUser<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
    }
}
