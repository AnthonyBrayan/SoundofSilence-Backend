using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SoundofSilence.IServices;
using SoundofSilence.Models;
using SoundofSilence.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Web.Http.Cors;

namespace SoundofSilence.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    //[EnableCors("AllowAll")]

    [Route("[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUsersService _usersService;
        private readonly IServiceContext _serviceContext;

        public UsersController(IConfiguration configuration, IUsersService usersService, IServiceContext serviceContext)
        {
            _configuration = configuration;
            _usersService = usersService;
            _serviceContext = serviceContext;
        }

        //[HttpPost(Name = "InsertUsers")]
        //public IActionResult Post([FromBody] Users users)
        //{
        //    try
        //    {
        //        var roleName = "Subscribe";
        //        var roleId = _usersService.GetRoleIdByName(roleName);

        //        users.Id_rol = roleId;

        //        var existingUserWithSameEmail = _serviceContext.Set<Users>()
        //            .FirstOrDefault(u => u.Email == users.Email);

        //        if (existingUserWithSameEmail != null)
        //        {
        //            return StatusCode(404, "Ya existe un usuario con el mismo correo electrónico.");
        //        }
        //        else
        //        {
        //            // Hash de la contraseña antes de almacenarla en la base de datos
        //            users.Password = BCrypt.Net.BCrypt.HashPassword(users.Password);

        //            return Ok(_usersService.InsertUsers(users));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error al obtener el ID del rol: {ex.Message}");
        //    }
        //}


        [HttpPost(Name = "InsertUsers")]
        public IActionResult Post([FromBody] Users users)
        {
            try
            {
                var roleName = "Subscribe";
                var roleId = _usersService.GetRoleIdByName(roleName);

                // Verifica si se proporcionó manualmente un valor para Id_rol en Swagger
                if (users.Id_rol == 0)
                {
                    // Si no se proporcionó un valor manualmente, establece el valor predeterminado (2)
                    users.Id_rol = 2;
                }

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
                    var token = GenerateJwtToken(user);

                    // Retorna el token y el rol del usuario en la respuesta
                    return Ok(new { Token = token, Role = user.Id_rol });
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
        private string GenerateJwtToken(Users user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id_user.ToString()),
                    // Otros claims si es necesario
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Duración del token
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }




        [HttpDelete("{id}", Name = "DeleteUser")]
        public IActionResult Delete(int id)
        {
            try
            {
                var user = _serviceContext.Users.FirstOrDefault(u => u.Id_user == id);

                if (user == null)
                {
                    return StatusCode(404, "Usuario no encontrado");
                }

                _serviceContext.Users.Remove(user);
                _serviceContext.SaveChanges();

                return NoContent(); // Devuelve HTTP 204 (No Content) para indicar que la eliminación fue exitosa.
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el usuario: {ex.Message}");
            }
        }
    }


}




