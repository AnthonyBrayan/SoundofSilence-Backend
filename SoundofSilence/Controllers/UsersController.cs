using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using SoundofSilence.IServices;
using SoundofSilence.Models;
using SoundofSilence.Services;
using System.Security.Authentication;
using System.Web.Http.Cors;

namespace SoundofSilence.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    //[EnableCors("AllowAll")]

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
        public IActionResult Post([FromBody] Users users)
        {
            try
            {
                var roleName = "Subscribe";
                var roleId = _usersService.GetRoleIdByName(roleName);

                users.Id_rol = roleId;

                var existingUserWithSameEmail = _serviceContext.Set<Users>()
                    .FirstOrDefault(u => u.Email == users.Email);

                if (existingUserWithSameEmail != null)
                {
                    return StatusCode(404, "Ya existe un usuario con el mismo correo electrónico.");
                }
                else
                {
                    // Hash de la contraseña antes de almacenarla en la base de datos
                    users.Password = BCrypt.Net.BCrypt.HashPassword(users.Password);

                    return Ok(_usersService.InsertUsers(users));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el ID del rol: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequestModel loginRequest)
        {
            try
            {
                var user = _serviceContext.Users.FirstOrDefault(u => u.Email == loginRequest.Email);

                if (user != null && BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
                {
                    return StatusCode(200, "Inicio de sesión exitoso");
                }
                else
                {
                    return StatusCode(401, "Credenciales incorrectas");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al iniciar sesión: {ex.Message}");
            }
        }


        //private string GenerateAuthToken(int userId)
        //{
        //    // Implementa la lógica para generar un token de autenticación, por ejemplo, utilizando JWT.
        //    // Retorna el token generado.
        //}

    }
}
