namespace PoC.DistributedAspNetIdentity.Api.Models
{
    public class CheckCredentialsRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
