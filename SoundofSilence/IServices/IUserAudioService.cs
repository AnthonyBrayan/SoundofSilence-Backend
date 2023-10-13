using Entities;

namespace SoundofSilence.IServices
{
    public interface IUserAudioService
    {

        int InsertUserAudio(UserAudio usersAudio);
        List<UserAudio> GetUserAudiosByUserId(int userId);
    }
}
