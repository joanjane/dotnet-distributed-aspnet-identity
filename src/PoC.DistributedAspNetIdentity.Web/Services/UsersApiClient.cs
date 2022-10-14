using PoC.DistributedAspNetIdentity.Web.Services.Dto;

namespace PoC.DistributedAspNetIdentity.Web.Services
{
    public class UsersApiClient : IUsersApiClient
    {
        private readonly HttpClient _httpClient;

        public UsersApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserResponse> CheckCredentials(CheckCredentialsRequest request, CancellationToken cancellationToken)
        {
            var route = "/api/users/check-credentials";
            var response = await _httpClient.PostAsJsonAsync(route, request, cancellationToken);
            await response.CheckErrorResponse(cancellationToken);

            return await response.Content.ReadFromJsonAsync<UserResponse>(cancellationToken: cancellationToken);
        }

        public async Task<UserResponse> CreateUser(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var route = "/api/users/";
            var response = await _httpClient.PostAsJsonAsync(route, request, cancellationToken);
            await response.CheckErrorResponse(cancellationToken);

            return await response.Content.ReadFromJsonAsync<UserResponse>(cancellationToken: cancellationToken);
        }
    }
}
