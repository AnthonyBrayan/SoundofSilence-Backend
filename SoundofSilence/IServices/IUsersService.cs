using Entities;

namespace SoundofSilence.IServices
{
    public interface IUsersService
    {
        int InsertUsers(Users users);
        int GetRoleIdByName(string roleName);
    }
}
