﻿using Data;
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


        public bool DeleteUser(int userId)
        {
            var user = _serviceContext.Users.FirstOrDefault(u => u.Id_user == userId);

            if (user == null)
            {
                return false; // Usuario no encontrado, la eliminación no se realizó.
            }

            _serviceContext.Users.Remove(user);
            _serviceContext.SaveChanges();

            return true; // La eliminación se realizó con éxito.
        }

    }
}
