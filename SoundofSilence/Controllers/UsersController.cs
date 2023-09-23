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
        public IActionResult Post([FromQuery] string userName, [FromQuery] string userPassword, [FromBody] Users users)
        {
            var selectedUser = _serviceContext.Set<Users>()
                                   .Where(u => u.Name_user == userName
                                       && u.Password == userPassword
                                       && u.Id_rol == 1)
                                    .FirstOrDefault();

            if (selectedUser != null)
            {

                var existingUserWithSameEmail = _serviceContext.Set<Users>()
                    .FirstOrDefault(u => u.Email == users.Email);

                if (existingUserWithSameEmail != null)
                {
                    return StatusCode(404, "Ya existe un usuario con el mismo correo electrónico.");
                }
                else
                {
                    return Ok(_usersService.InsertUsers(users));
                }

            }
            else
            {
                return StatusCode(404, "El usuario no está autorizado o no existe");
            }
        }
    }
}
