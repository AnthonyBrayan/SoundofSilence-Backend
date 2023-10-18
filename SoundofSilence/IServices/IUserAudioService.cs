﻿using Entities;

namespace SoundofSilence.IServices
{
    public interface IUserAudioService
    {
        int InsertUserAudio(UserAudio userAudio);
        UserAudio GetUserAudioByUserIdAndCardId(int userId, int cardId);
        void RemoveUserAudio(int userAudioId);
       
    }
}
