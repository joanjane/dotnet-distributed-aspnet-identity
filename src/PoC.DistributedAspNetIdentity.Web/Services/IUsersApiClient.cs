using PoC.DistributedAspNetIdentity.Web.Services.Dto;

namespace PoC.DistributedAspNetIdentity.Web.Services
{
    public interface IUsersApiClient
    {
        Task<UserResponse> CheckCredentials(CheckCredentialsRequest request, CancellationToken cancellationToken);
        Task<UserResponse> CreateUser(CreateUserRequest request, CancellationToken cancellationToken);
    }
}