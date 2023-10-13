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

        public int InsertUserAudio(UserAudio usersAudio)
        {
            _serviceContext.UserAudio.Add(usersAudio);
            _serviceContext.SaveChanges();
            return usersAudio.Id_UserAudio;
        }

        public List<UserAudio> GetUserAudiosByUserId(int userId)
        {
            // Utiliza LINQ para filtrar UserAudio según el Id_user
            var userAudios = _serviceContext.UserAudio
                .Where(ua => ua.Id_user == userId)
                .ToList();

            return userAudios;
        }


    }
}
