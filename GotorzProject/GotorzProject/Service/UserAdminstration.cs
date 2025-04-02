using GotorzProject.Model;
using GotorzProject.Shared.DataTransfer;
using Microsoft.AspNetCore.Identity;
using NuGet.Protocol.Plugins;
using System.Collections;
namespace GotorzProject.Service
{
    // This service is and should only be available to Admin users
    public class UserAdminstration : IUserAdminstration
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



        UserManager<CustomUser> _manager;

        // Maybe in future we add custom roles?
        RoleManager<IdentityRole> _roleManager;

        public UserAdminstration(UserManager<CustomUser> manager, RoleManager<IdentityRole> roleManager)
        {
            _manager = manager;
            _roleManager = roleManager;
        }

        public async Task SetupRoles()
        {
            foreach (var role in Roles.Values)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        // Promote user to customer! :D
        public async Task RemoveRolesFromUser(CustomUser user)
        {
            // Don't allow admins to remove other admins... [coup d'état] not allowed
            await _manager.RemoveFromRolesAsync(user, Roles.Values.Where(type => type != "Admin"));
        }

        // remove specific role from user, anthing but admin at least
        public async Task RemoveRoleFromUser(CustomUser user, string role)
        {
            if (role != "Admin" || Roles.ContainsKey(role))
                await _manager.RemoveFromRoleAsync(user, role);
        }

        public async Task<List<string>> GetUserRoles(CustomUser user)
        {
            return new(await _manager.GetRolesAsync(user));
        }

        // Give user single role
        public async Task SetRole(CustomUser user, string role)
        {
            // Admins should only be set through database for security
            // Or with specialized method that takes a secret passphrase
            if (Roles.ContainsKey(role) && role != "Admin")
            {
                await _manager.AddToRoleAsync(user, role);
            }
        }

        private async Task<IEnumerable<string>> trimRoles(CustomUser user, IEnumerable<string> roles)
        {
            var trimmedRoles = new List<string>();

            foreach (var role in roles)
            {
                if (!await _manager.IsInRoleAsync(user, role))
                {
                    trimmedRoles.Add(role);
                }
            }
            // Also remove non-valid roles
            var result = trimmedRoles.Where(role => Roles.ContainsKey(role));

            return await Task.FromResult(result);
        }

     
        public async Task SetRoles(CustomUser user, IEnumerable<string> roles)
        {
            if (roles.Any(role => role == "Admin"))
            {
                return;
            }

            var trimmedRoles = await trimRoles(user, roles);

            var result = await _manager.AddToRolesAsync(user, trimmedRoles.Where(role => Roles.ContainsKey(role)));
            Console.WriteLine($"User roles change {result.Succeeded}");
            if (result.Errors.Count() > 0)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"{error.Code} {error.Description}");
                }
            }
        }

        // only accessable by admin
        private async Task AdminSetRoles(CustomUser user, IEnumerable<string> roles)
        {
            var trimmedRoles = await trimRoles(user, roles);

            var result = await _manager.AddToRolesAsync(user, trimmedRoles.Where(role => Roles.ContainsKey(role)));

            Console.WriteLine($"User roles change {result.Succeeded}");
            if (result.Errors.Count()>0)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"{error.Code} {error.Description}");
                }
            }
        }

        public async Task<List<CustomUser>> GetUsersByRole(string role)
        {
            var temp = (await _manager.GetUsersInRoleAsync(role));
            return new(temp);
        }



        public async Task<Dictionary<string, List<CustomUser>>> GetAllUsersDict()
        {
            Dictionary<string, List<CustomUser>> result = new();


            foreach (string role in Roles.Keys)
            {
          
                List<CustomUser> idenUsers = await GetUsersByRole(role);
                result.Add(role, idenUsers);
            }

            return result;
        }

        public async Task<List<CustomUser>> GetAllUsersList()
        {
            List<CustomUser> result = new();
            foreach (string role in Roles.Keys)
            {
                result.AddRange(await GetUsersByRole(role));
            }

            return result;
        }


        public async Task<bool> UpdateUser(UserDTO user, bool isAdminChange)
        {
            
            CustomUser? customUser = await _manager.FindByEmailAsync(user.Email);
            if(customUser == null)
            {
                return await Task.FromResult(false);
            }

            customUser.TelephoneNumber = user.TelephoneNumber;
            //customUser.Email = user.Email; We do NOT change this one 
            // Do not "correct" this, this is how it is supposed to be.
            customUser.FirstName = user.FirstName;
            customUser.LastName = user.LastName;
            customUser.ProfilePictureSrc = user.ProfilePicture;
            customUser.Address = user.Address;
            customUser.Country = user.Country;

            var idenResult = await _manager.UpdateAsync(customUser);

            if (idenResult.Succeeded)
            {
                // a bug here could wipe user roles, funny stuff


                await RemoveRolesFromUser(customUser);

                if(user.Roles != null)
                {
                    // change roles after we know that other changes went through
                    if (isAdminChange)
                    {
                        foreach (var role in user.Roles)
                        {
                            Console.WriteLine($"ADMIN ADMIN Adding to : {user.Email} role : {role}");
                        }
                        await AdminSetRoles(customUser, user.Roles);
                    }
                    else
                    {
                        foreach(var role in user.Roles)
                        {
                            Console.WriteLine($"Adding to : {user.Email} role : {role}");
                        }
                        await SetRoles(customUser, user.Roles);
                    }
                }

                return await Task.FromResult(true);
            }
            else
            {
                return await Task.FromResult(false);
            }
        }

        public async Task<List<string>> GetRoles()
        {
            return await Task.FromResult(Roles.Select(r => r.Value).ToList());
        }
    }
}
