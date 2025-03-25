using Microsoft.AspNetCore.Identity;
namespace GotorzProject.Service
{
    // This service is and should only be available to Admin users
    public class UserAdminstration
    {

        public static readonly Dictionary<string, string> Roles = new()
        {
            { "Admin", "Admin" },
            { "Manager", "Manager" },
            { "Support", "Support" },
            { "Sales", "Sales" }
        };

        UserManager<IdentityUser> _manager;

        public UserAdminstration(UserManager<IdentityUser> manager)
        {
            _manager = manager;
        }

        // Promote user to customer! :D
        public async Task RemoveRolesFromUser(IdentityUser user)
        {
            // Don't allow admins to remove other admins... [coup d'état] not allowed
            await _manager.RemoveFromRolesAsync(user, Roles.Values.Where(type => type != "Admin"));
        }

        public async Task SetRole(IdentityUser user, string role)
        {
            // Admins should only be set through database for security
            // Or with specialized method that takes a secret passphrase
            if (Roles.ContainsKey(role) && role!="Admin")
            {
                await _manager.AddToRoleAsync(user, role);
            }
        }

        public async Task SetRoles(IdentityUser user, IEnumerable<string> roles)
        {
            if (roles.Any(role => role=="Admin"))
            {
                
            }
            foreach (var role in roles)
            {
                // Admins should only be set through database for security
                // Or with specialized method that takes a secret passphrase
                if (Roles.ContainsKey(role))
                {

                }
            }
        }

    }
}
