using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using SoundofSilence.IServices;
using System.Security.Authentication;
using System.Web.Http.Cors;

namespace SoundofSilence.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("[controller]/[action]")]
    public class RolCcontroller : ControllerBase
    {

        private readonly IRolService _rolService;
        private readonly ServiceContext _serviceContext;

        public RolCcontroller(IRolService rolService, ServiceContext serviceContext)
        {
            _rolService = rolService;
            _serviceContext = serviceContext;

        }

        [HttpPost(Name = "InsertRol")]
        public int Post([FromQuery] string userName, [FromQuery] string userPassword, [FromBody] Rol rol)
        {
            var selectedUser = _serviceContext.Set<Users>()
                                   .Where(u => u.Name_user == userName
                                       && u.Password == userPassword
                                       && u.Id_rol == 1)
                                    .FirstOrDefault();

            if (selectedUser != null)
            {
                return _rolService.InsertRol(rol);
            }
            else
            {
                throw new InvalidCredentialException("El usuario no está autorizado o no existe");
            }
        }



    }
}
