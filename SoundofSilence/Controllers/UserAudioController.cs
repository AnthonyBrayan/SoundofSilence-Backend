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
        //private readonly ITokenHandler _tokenHandler;

        public UserAudioController(IConfiguration configuration, IUserAudioService userAudioService)
        {
            _configuration = configuration;
            _userAudioService = userAudioService;
            //_tokenHandler = tokenHandler;

        }

        [HttpPost("MarkFavorite")]
        public IActionResult MarkFavorite([FromBody] MarkFavoriteModel model)

        {

            try
            {
                Console.WriteLine("IdCArd recibido: " + model.cardId);

                var userId = ExtractUserIdFromAuthorizationHeader(HttpContext);


                if (userId == null)
                {
                    // El usuario no está autenticado
                    return Unauthorized("El usuario no está autenticado.");
                }

                // Realiza una conversión segura a int
                int idUser = userId.Value;
                // //Verifica si el usuario ya ha marcado esta tarjeta como favorita
                var existingUserAudio = _userAudioService.GetUserAudioByUserIdAndCardId(idUser, model.cardId);
                if (existingUserAudio != null)
                {
                    // La tarjeta ya está marcada como favorita, puedes manejar esto como desmarcarla
                    // Aquí, puedes cambiar el estado o eliminar el registro, según tu modelo
                    _userAudioService.RemoveUserAudio(existingUserAudio.Id_UserAudio);
                    return Ok("Tarjeta desmarcada como favorita.");
                }
                else
                {
                    // La tarjeta aún no está marcada como favorita, puedes agregar un nuevo registro en UserAudio
                    var userAudio = new UserAudio
                    {
                        Id_user = idUser,
                        Id_AudioFiles = model.cardId,
                        State = "active" // Puedes usar otro estado si lo prefieres
                    };

                    var userAudioId = _userAudioService.InsertUserAudio(userAudio);

                    return Ok($"Tarjeta marcada como favorita con ID {userAudioId}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en MarkFavorite: " + ex.ToString());
                return StatusCode(500, "Ocurrió un error interno en el servidor.");
            }
        }

        [HttpGet("favoritos")]
        public async Task<IActionResult> GetFavoritos()
        {
            var userId = ExtractUserIdFromAuthorizationHeader(HttpContext);
            //var userId = ExtractUserIdFromToken(HttpContext);


            if (userId == null)
            {
                // El usuario no está autenticado
                return Unauthorized("El usuario no está autenticado.");
            }

            // Realiza una conversión segura a int
            int idUser = userId.Value;

            return Ok(_userAudioService.GetFavoriteAudioFilesByUserId(idUser));
        }



        private int? ExtractUserIdFromToken(HttpContext httpContext)
        {
            var token = httpContext.Request.Cookies["jwtToken"];

            // Agrega un registro para imprimir el contenido del token
            Console.WriteLine("Token recibido: " + token);

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
            // Configura la validación de tokens
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false, // Puedes ajustar esto según tu configuración
                ValidateAudience = false, // Puedes ajustar esto según tu configuración
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"])), // Reemplaza con tu clave secreta
            };

            try
            {
                // Crea un token handler
                var tokenHandler = new JwtSecurityTokenHandler();

                // Valida y deserializa el token
                var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                // Extrae el ID de usuario del token
                var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name); // "sub" es la claim común para el ID de usuario

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }
            }
            catch (SecurityTokenExpiredException ex)
            {
                // El token ha expirado, puedes manejar esto de acuerdo a tus necesidades
                Console.WriteLine("El token ha expirado: " + ex.Message);
            }
            catch (SecurityTokenInvalidSignatureException ex)
            {
                // La firma del token no es válida, puedes manejar esto de acuerdo a tus necesidades
                Console.WriteLine("La firma del token no es válida: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Manejo genérico para otras excepciones
                Console.WriteLine("Error al descodificar o validar el token: " + ex.Message);
            }

            return null;
        }


        public class MarkFavoriteModel
        {
            public int cardId { get; set; }
        }





    }
}
