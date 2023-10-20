using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SoundofSilence.IServices;
using SoundofSilence.Models;
using SoundofSilence.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.Web.Http.Cors;

namespace SoundofSilence.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
   

    [Route("[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUsersService _usersService;
        private readonly ServiceContext _serviceContext;

        public UsersController(IConfiguration configuration, IUsersService usersService, ServiceContext serviceContext)
        {
            _configuration = configuration;
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

                
                if (users.Id_rol == 0)
                {
                    
                    users.Id_rol = 2;
                }

                var existingUserWithSameEmail = _serviceContext.Users.FirstOrDefault(u => u.Email == users.Email);

                if (existingUserWithSameEmail != null)
                {
                    return StatusCode(409, "A user with the same email address already exists.");
                }
                else
                {
                    
                    users.Password = BCrypt.Net.BCrypt.HashPassword(users.Password);

                    return Ok (_usersService.InsertUsers(users));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting role ID: {ex.Message}");
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

                    
                    Response.Cookies.Append("jwtToken", token, new CookieOptions
                    {
                        HttpOnly = false, 
                                          
                    });

                    return Ok(new { Token = token, Role = user.Id_rol });

                   
                }
                else
                {
                    return StatusCode(401, "Incorrect credentials");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error logging in: {ex.Message}");
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
                    
                }),
                Expires = DateTime.UtcNow.AddHours(1),
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
                    return StatusCode(404, "User not found");
                }

                _serviceContext.Users.Remove(user);
                _serviceContext.SaveChanges();

                return NoContent(); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting user: {ex.Message}");
            }
        }
    }
}
