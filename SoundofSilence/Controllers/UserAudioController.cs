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

namespace SoundofSilence.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("[controller]/[action]")]
    public class UserAudioController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserAudioService _userAudioService;
        private readonly IAudioFilesService _audioFilesService;
        

        private readonly ServiceContext _serviceContext;

        public UserAudioController(IConfiguration configuration, IUserAudioService userAudioService, IAudioFilesService audioFilesService, ServiceContext serviceContext)
        {
            _configuration = configuration;
            _userAudioService = userAudioService;
            _serviceContext = serviceContext;

        }

        //[HttpPost(Name = "InsertUserAudioFile")]
        //public IActionResult PostAudioFile( int Id_AudioFiles, [FromBody] UserAudio userAudio)
        //{
        //    try
        //    {
        //        // Verifica si el Id_AudioFiles existe en la base de datos
        //        var audioFileExists = _audioFilesService.Exists(Id_AudioFiles);

        //        if (!audioFileExists)
        //        {
        //            return NotFound($"No se encontró un archivo de audio con el ID: {Id_AudioFiles}");
        //        }

        //        // Aquí debes validar y mapear los datos del modelo al tipo de entidad que utiliza tu servicio.
        //        var userAudioEntity = new UserAudio
        //        {
        //            State = userAudio.State,
        //            Id_user = userAudio.Id_user,
        //            Id_AudioFiles = userAudio.Id_AudioFiles,
        //        };

        //        // Utiliza tu servicio para insertar el registro en la base de datos.
        //        var userAudioId = _userAudioService.InsertUserAudio(userAudioEntity);

        //        return Ok($"Registro de UserAudio insertado con éxito. ID: {userAudioId}");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error al insertar el registro de UserAudio: {ex.Message}");
        //    }


        //}

        [HttpPost("MarkFavorite")]
        //public IActionResult MarkFavorite([FromBody] int cardId, [FromBody] string token)
        public IActionResult MarkFavorite(int cardId, [FromBody] string token)
        {
            // Recupera el ID del usuario desde el token almacenado en las cookies
            var userId = ExtractUserIdFromToken(HttpContext);

            if (userId == null)
            {
                // El usuario no está autenticado
                return Unauthorized("El usuario no está autenticado.");
            }
            // Realiza una conversión segura a int
            int idUser = userId.Value;
            // //Verifica si el usuario ya ha marcado esta tarjeta como favorita
            var existingUserAudio = _userAudioService.GetUserAudioByUserIdAndCardId(idUser, cardId);
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
                    Id_AudioFiles = cardId,
                    State = "active" // Puedes usar otro estado si lo prefieres
                };

                var userAudioId = _userAudioService.InsertUserAudio(userAudio);

                return Ok($"Tarjeta marcada como favorita con ID {userAudioId}.");
            }
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
            catch (Exception ex)
            {
                // Maneja errores de decodificación o validación aquí
            }

            return null;
        }






    }
}
