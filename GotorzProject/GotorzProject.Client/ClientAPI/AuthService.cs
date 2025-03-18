using System.Net.Http.Json;

namespace GotorzProject.Client.ClientAPI
{
    public class AuthService
    {
        HttpClient? _httpClient;

        private string apiBase = "/api/authentication/";

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //public AuthService()
        //{

        //}

        public async Task<bool> AsyncLogin(string email, string password)
        {
            if (_httpClient == null)
            {
                throw new Exception("AuthService not properly setup.");
            }
            string apiEndpoint = apiBase + "login";

            var requestBody = new { Email = email, Password = password };

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

        public async Task<bool> AsyncRegister(string email, string password, string firstname, string lastname, string country, string postalcode, string phonenumber, string address)
        {
            if (_httpClient == null)
            {
                throw new Exception("AuthService not properly setup.");
            }
            string apiEndpoint = apiBase + "register";

            var requestBody = new { Email = email, Password = password, FirstName = firstname, LastName = lastname, Country = country, Address = address, PhoneNumber = phonenumber };

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
