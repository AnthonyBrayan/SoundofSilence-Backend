using Data;
using Entities;
using SoundofSilence.IServices;

namespace SoundofSilence.Services
{
    public class UsersService : BaseContextService, IUsersService
    {
        public UsersService(ServiceContext serviceContext) : base(serviceContext)
        {
        }

        public int InsertUsers(Users users)
        {
            _serviceContext.Users.Add(users);
            _serviceContext.SaveChanges();
            return users.Id_user;
        }

        public int GetRoleIdByName(string roleName)
        {
            var role = _serviceContext.Rol.FirstOrDefault(r => r.Name_rol == roleName);

            if (role != null)
            {
                return role.Id_rol;
            }
            else
            {
                throw new Exception($"No se encontró un rol con el nombre {roleName}");
            }
        }

    }
}
