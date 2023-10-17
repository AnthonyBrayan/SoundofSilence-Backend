using Entities;

namespace SoundofSilence.IServices
{
    public interface IAudioFilesService
    {
        int InsertAudioFiles(AudioFiles audioFiles);
        void UpdateAudioFiles(int Id_AudioFiles, AudioFiles updatedAudioFiles);
        void DeleteAudioFile(int Id_AudioFiles);
        List<AudioFiles> GetAudioFiles();
        List<AudioFiles> GetAudioFilesByCategory(int Id_category);
    }
}
