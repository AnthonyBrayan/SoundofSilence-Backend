using Data;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;    
using SoundofSilence.IServices;
using SoundofSilence.Services;
using System.Security.Authentication;
using System.Security.Claims;
using System.Web.Http.Cors;

using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;



namespace SoundofSilence.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("[controller]/[action]")]
    public class UserAudioController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserAudioService _userAudioService;
       

        public UserAudioController(IConfiguration configuration, IUserAudioService userAudioService)
        {
            _configuration = configuration;
            _userAudioService = userAudioService;
            

        }

        [HttpPost("MarkFavorite")]
        public IActionResult MarkFavorite([FromBody] MarkFavoriteModel model)

        {

            try
            {
                Console.WriteLine("IdCArd received: " + model.cardId);

                var userId = ExtractUserIdFromAuthorizationHeader(HttpContext);
                



                if (userId == null)
                {
                    
                    return Unauthorized("User is not authenticated.");
                }

                
                int idUser = userId.Value;
                
                var existingUserAudio = _userAudioService.GetUserAudioByUserIdAndCardId(idUser, model.cardId);
                if (existingUserAudio != null)
                {
                    
                    _userAudioService.RemoveUserAudio(existingUserAudio.Id_UserAudio);
                    return Ok("Card unmarked as favourite.");
                }
                else
                {
                    var userAudio = new UserAudio
                    {
                        Id_user = idUser,
                        Id_AudioFiles = model.cardId,
                        State = "active" 
                    };

                    var userAudioId = _userAudioService.InsertUserAudio(userAudio);

                    return Ok($"Favourite card with ID {userAudioId}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in MarkFavorite: " + ex.ToString());
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpGet("favoritos")]
        public async Task<IActionResult> GetFavoritos()
        {
            var userId = ExtractUserIdFromAuthorizationHeader(HttpContext);
           


            if (userId == null)
            {
                
                return Unauthorized("User is not authenticated.");
            }

            
            int idUser = userId.Value;

            return Ok(_userAudioService.GetFavoriteAudioFilesByUserId(idUser));
        }



        private int? ExtractUserIdFromToken(HttpContext httpContext)
        {
            var token = httpContext.Request.Cookies["jwtToken"];

           
            Console.WriteLine("Token received: " + token);

            if (token != null)
            {
                var userId = DecodeUserIdFromToken(token);
                return userId;
            }
            return null;
        }

        private int? ExtractUserIdFromAuthorizationHeader(HttpContext httpContext)
        {
            var authorizationHeader = httpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();
                var userId = DecodeUserIdFromToken(token);
                return userId;
            }

            return null;
        }

        private int? DecodeUserIdFromToken(string token)
        {
           
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false, 
                ValidateAudience = false, 
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"])),
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                
                var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }
            }
            catch (SecurityTokenExpiredException ex)
            {
                
                Console.WriteLine("The token has expired: " + ex.Message);
            }
            catch (SecurityTokenInvalidSignatureException ex)
            {
               
                Console.WriteLine("Token signature is invalid: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error decoding or validating token: " + ex.Message);
            }

            return null;
        }


        public class MarkFavoriteModel
        {
            public int cardId { get; set; }
        }





    }
}
