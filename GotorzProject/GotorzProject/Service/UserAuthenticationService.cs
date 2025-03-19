using Azure;
using GotorzProject.Model;
using GotorzProject.Model.Auth;
using GotorzProject.Model.ObjectRelationMapping;
using GotorzProject.ServerAPI;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.Configuration;
using System.Security.Cryptography;

namespace GotorzProject.Service
{
    public class UserAuthenticationService
    {
        private ApplicationIdentityDbContext _context;

        public UserAuthenticationService(ApplicationIdentityDbContext context)
        {
            _context = context; 
        }
        /**
         * returns string token if successfull login, null if not
         */
        public async Task<string?> AsyncLogin(string email, string password)
        {
            if (_context == null)
            {
                throw new InvalidConfigurationException("Bad configuration, code is ass, terminating session.");
            }

            // username is email
            // todo : change all places to say email instead of username
            var user = await _context.Customers.FirstOrDefaultAsync((usr) => usr.Email == email);

            if (user != null)
            {
                // verify password matches stored password
                string hash = user.Password ?? "";

                bool correctPassword = BCrypt.Net.BCrypt.Verify(password, hash);
                if (correctPassword)
                {

                    // Todo : check if token for user already exists, then delete it 
                    string token = GenerateToken();

                    CustomToken cToken = new()
                    {
                        Created = DateTime.UtcNow,
                        Expires = DateTime.UtcNow.AddHours(1),
                        Assignee =  user,
                        Key = token
                    };

                    _context.CustomTokens.Add(cToken);
                    _context.SaveChanges();

                    // valid login attempt
                    return await Task.FromResult(token);
                }
            }

            // Invalid login attempt
            return await Task.FromResult<string>(null);
        }

        public async Task<bool> Register(Customer customer)
        {
            string email = customer.Email;
            
            if (_context == null)
            {
                throw new InvalidConfigurationException("Bad configuration, code is ass, terminating session.");
            }

            bool exists = _context.Customers.Any((cust) => cust.Email == email);
            if (exists)
            {
                return await Task.FromResult(false);
            }

            _context.Customers.Add(customer);

            try
            {
                _context.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(false);
            }
        }

        private static string GenerateToken()
        {

            byte[] buffer = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buffer);
            }
            return Convert.ToBase64String(buffer);
        }

    }
}
