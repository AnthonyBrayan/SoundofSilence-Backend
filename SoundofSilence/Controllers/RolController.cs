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
    public class RolController : ControllerBase
    {

        private readonly IRolService _rolService;
        private readonly ServiceContext _serviceContext;

        public RolController(IRolService rolService, ServiceContext serviceContext)
        {
            _rolService = rolService;
            _serviceContext = serviceContext;

        }

        [HttpPost(Name = "InsertRol")]
        public IActionResult Post([FromQuery] string userName, [FromQuery] string userPassword, [FromBody] Rol rol)
        {
            var selectedUser = _serviceContext.Set<Users>()
                                   .Where(u => u.Name_user == userName
                                       && u.Password == userPassword
                                       && u.Id_rol == 1)
                                    .FirstOrDefault();

            if (selectedUser != null)
            {
                var existingWithNameRol = _serviceContext.Set<Rol>()
                    .FirstOrDefault(u => u.Name_rol == rol.Name_rol);

                if (existingWithNameRol != null)
                {
                    return StatusCode(404, "Ya existe un rol con el mismo nombre.");
                }
                else
                {
                    return Ok(_rolService.InsertRol(rol));
                }
            }
            else
            {
                return StatusCode(404, "El usuario no está autorizado o no existe");
            }
        }

    }
}
