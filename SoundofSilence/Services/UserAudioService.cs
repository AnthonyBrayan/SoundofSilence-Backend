using Data;
using Entities;
using SoundofSilence.IServices;

namespace SoundofSilence.Services
{
    public class UserAudioService : BaseContextService, IUserAudioService
    {
        public UserAudioService(ServiceContext serviceContext) : base(serviceContext)
        {

        }

        public int InsertUserAudio(UserAudio userAudio)
        {
            _serviceContext.UserAudio.Add(userAudio);
            _serviceContext.SaveChanges();
            return userAudio.Id_UserAudio; // Devuelve el ID del nuevo registro
        }

        public UserAudio GetUserAudioByUserIdAndCardId(int userId, int cardId)
        {
            // Realiza una consulta en tu base de datos para encontrar un registro en la tabla UserAudio
            // que coincida con el userId y el cardId
            var userAudio = _serviceContext.UserAudio
                .FirstOrDefault(ua => ua.Id_user == userId && ua.Id_AudioFiles == cardId);

            return userAudio;
        }

        public void RemoveUserAudio(int userAudioId)
        {
            // Busca el registro de UserAudio por su Id_UserAudio
            var userAudio = _serviceContext.UserAudio.FirstOrDefault(ua => ua.Id_UserAudio == userAudioId);

            if (userAudio != null)
            {
                // Si se encuentra el registro, lo elimina de la base de datos
                _serviceContext.UserAudio.Remove(userAudio);
                _serviceContext.SaveChanges();
            }
            // Si el registro no existe, puedes manejarlo de alguna manera, por ejemplo, lanzar una excepción o realizar otro tipo de acción.
            else
            {
                // Manejo de registro no encontrado, por ejemplo:
                throw new Exception("El registro de UserAudio no se encontró para el Id proporcionado.");
            }
        }


    }
}
