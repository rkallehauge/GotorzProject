using System.Net.Http.Json;

namespace GotorzProject.Client.ClientAPI
{
    public class AuthService
    {
        HttpClient? _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //public AuthService()
        //{

        //}

        public async Task<bool> AsyncLogin(string username, string password)
        {
            if (_httpClient == null)
            {
                throw new Exception("AuthService not properly setup.");
            }
            string apiEndpoint = "/api/Login";

            var requestBody = new { Username = username, Password = password };

            var response = await _httpClient.PostAsJsonAsync(apiEndpoint, requestBody);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();

                // todo : store token

                return true;
            }
            else
            {
                // Throw exception ? 
                return false;
            }
        }

        public async Task<bool> AsyncRegister(string username, string password)
        {
            if (_httpClient == null)
            {
                throw new Exception("AuthService not properly setup.");
            }
            string apiEndpoint = "/api/Register";

            var requestBody = new { Username = username, Password = password };

            var response = await _httpClient.PostAsJsonAsync(apiEndpoint, requestBody);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();

                // todo : store token

                return true;
            }
            else
            {
                // Throw exception ? 
                return false;
            }
        }
    }
}
