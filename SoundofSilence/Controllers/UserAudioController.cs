using Data;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoundofSilence.IServices;
using SoundofSilence.Services;
using System.Security.Authentication;
using System.Security.Claims;
using System.Web.Http.Cors;

namespace SoundofSilence.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("[controller]/[action]")]
    public class UserAudioController : ControllerBase
    {

        private readonly IUserAudioService _userAudioService;
        private readonly ServiceContext _serviceContext;

        public UserAudioController(IUserAudioService userAudioService, ServiceContext serviceContext)
        {
            _userAudioService = userAudioService;
            _serviceContext = serviceContext;

        }

        [HttpPost(Name = "InsertUserAudio")]
        public IActionResult Post([FromBody] UserAudio userAudio)
        {
            return Ok(_userAudioService.InsertUserAudio(userAudio));
        }

        [HttpGet("user-audios")]
        [Authorize] // Asegura que solo los usuarios autenticados puedan acceder a esta API
        public IActionResult GetUserAudios()
        {
            var usuarioAutenticado = User;  // Esto es un objeto ClaimsPrincipal
            var userId = usuarioAutenticado.FindFirst(ClaimTypes.NameIdentifier)?.Value;  // Obtiene el ID del usuario
            //// Obtiene el ID del usuario desde el token JWT
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("No se pudo obtener el ID del usuario.");
            }

            var userAudios = _userAudioService.GetUserAudiosByUserId(int.Parse(userId));
            return Ok(userAudios);
        }

    }
}
