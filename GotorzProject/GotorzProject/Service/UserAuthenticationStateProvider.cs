using Microsoft.AspNetCore.Components.Authorization;

namespace GotorzProject.Service
{
    public class UserAuthenticationStateProvider : AuthenticationStateProvider
    {
        HttpClient _httpClient;

        public UserAuthenticationStateProvider(HttpClient client)
        {
                _httpClient = client;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            throw new NotImplementedException();   
        }
    }

    public class UserAuthData
    {
        public string Email { get; set; }
        public string Role { get; set; }
}
