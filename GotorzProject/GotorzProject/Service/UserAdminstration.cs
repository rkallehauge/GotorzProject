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

        // todo : get from config
        private const string secret = "{4100003d-3119-4f6c-a9d6-0f3024b9c65b}";

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

        // remove specific role from user, anthing but admin at least
        public async Task RemoveRoleFromUser(IdentityUser user, string role)
        {
            if(role!="Admin" || Roles.ContainsKey(role))
                await _manager.RemoveFromRoleAsync(user, role);
        }

        // Give user single role
        public async Task SetRole(IdentityUser user, string role)
        {
            // Admins should only be set through database for security
            // Or with specialized method that takes a secret passphrase
            if (Roles.ContainsKey(role) && role!="Admin")
            {
                await _manager.AddToRoleAsync(user, role);
            }
        }

        // Give user list of roles
        public async Task SetRoles(IdentityUser user, IEnumerable<string> roles)
        {
            if (roles.Any(role => role=="Admin"))
            {
                return;
            }
            await _manager.AddToRolesAsync(user, roles.Where(role => Roles.ContainsKey(role)));
        }

        public List<IdentityUser> GetUsersByRole(string role)
        {
            throw new NotImplementedException();
        }


        
        public Dictionary<string, List<IdentityUser>> GetAllUsersDict()
        {
            Dictionary<string, List<IdentityUser>> result = new();

            foreach(string role in Roles.Keys)
            {
                List<IdentityUser> idenUsers = GetUsersByRole(role);
                result.Add(role, idenUsers);
            }

            return result;
        }

        public List<IdentityUser> GetAllUsersList()
        {
            List<IdentityUser> result = new();
            foreach(string role in Roles.Keys)
            {
                result.Concat(GetUsersByRole(role));
            }

            return result;
        }
    }
}
