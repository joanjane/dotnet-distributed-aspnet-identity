namespace PoC.DistributedAspNetIdentity.Web.Services.Dto
{
    public class CheckCredentialsRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}