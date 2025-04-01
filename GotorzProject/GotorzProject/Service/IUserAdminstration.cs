using GotorzProject.Model;
using GotorzProject.Shared.DataTransfer;

namespace GotorzProject.Service
{
    public interface IUserAdminstration
    {
        Task<Dictionary<string, List<CustomUser>>> GetAllUsersDict();
        Task<List<CustomUser>> GetAllUsersList();
        Task<List<string>> GetUserRoles(CustomUser user);
        Task<List<CustomUser>> GetUsersByRole(string role);
        Task RemoveRoleFromUser(CustomUser user, string role);
        Task RemoveRolesFromUser(CustomUser user);
        Task SetRole(CustomUser user, string role);
        Task SetRoles(CustomUser user, IEnumerable<string> roles);
        Task SetupRoles();
        Task<bool> UpdateUser(UserDTO user, bool isAdminChange);
        Task<List<string>> GetRoles();
    }
}