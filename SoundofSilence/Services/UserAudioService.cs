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
            return userAudio.Id_UserAudio;
        }

        public UserAudio GetUserAudioByUserIdAndCardId(int userId, int cardId)
        {
           
            var userAudio = _serviceContext.UserAudio
                .FirstOrDefault(ua => ua.Id_user == userId && ua.Id_AudioFiles == cardId);

            return userAudio;
        }

        public void RemoveUserAudio(int userAudioId)
        {
            
            var userAudio = _serviceContext.UserAudio.FirstOrDefault(ua => ua.Id_UserAudio == userAudioId);

            if (userAudio != null)
            {
                
                _serviceContext.UserAudio.Remove(userAudio);
                _serviceContext.SaveChanges();
            }
            
            else
            {
                
                throw new Exception("UserAudio record was not found for the Id provided..");
            }
        }

        public List<UserAudio> GetUserAudio()
        {
            return _serviceContext.UserAudio.ToList();
        }

        public List<AudioFiles> GetFavoriteAudioFilesByUserId(int userId)
        {
         
            var query = from userAudio in _serviceContext.UserAudio
                        where userAudio.Id_user == userId
                        join audioFile in _serviceContext.AudioFiles on userAudio.Id_AudioFiles equals audioFile.Id_AudioFiles
                        select audioFile;

            
            return query.ToList();
        }




    }
}
