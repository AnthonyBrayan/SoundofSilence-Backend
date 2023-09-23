using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using SoundofSilence.IServices;
using SoundofSilence.Services;
using System.Security.Authentication;
using System.Web.Http.Cors;

namespace SoundofSilence.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly ServiceContext _serviceContext;

        public UsersController(IUsersService usersService, ServiceContext serviceContext)
        {
            _usersService = usersService;
            _serviceContext = serviceContext;
        }


        [HttpPost(Name = "InsertUsers")]
        public int Post([FromQuery] string userName, [FromQuery] string userPassword, [FromBody] Users users)
        {
            var selectedUser = _serviceContext.Set<Users>()
                                   .Where(u => u.Name_user == userName
                                       && u.Password == userPassword
                                       && u.Id_rol == 1)
                                    .FirstOrDefault();

            if (selectedUser != null)
            {

                // Verificar si ya existe un usuario con el mismo correo electrónico
                var existingUserWithSameEmail = _serviceContext.Set<Users>()
                    .FirstOrDefault(u => u.Email == users.Email);

                if (existingUserWithSameEmail != null)
                {
                    throw new InvalidCredentialException("Ya existe un usuario con el mismo correo electrónico.");
                }
                else
                {
                    // Si no existe un usuario con el mismo nombre de usuario ni correo electrónico,
                    // entonces puedes agregar el nuevo usuario.
                    return _usersService.InsertUsers(users);
                }

            }
            else
            {
                throw new InvalidCredentialException("El usuario no está autorizado o no existe");
            }
        }
    }
}
