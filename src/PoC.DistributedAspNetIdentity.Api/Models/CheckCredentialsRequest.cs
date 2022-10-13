namespace PoC.DistributedAspNetIdentity.Api.Models
{
    public class CheckCredentialsRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
