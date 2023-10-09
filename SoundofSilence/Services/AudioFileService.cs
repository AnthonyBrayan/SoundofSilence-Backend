using Data;
using Entities;
using SoundofSilence.IServices;

namespace SoundofSilence.Services
{
    public class AudioFileService : BaseContextService, IAudioFilesService
    {
        public AudioFileService(ServiceContext serviceContext) : base(serviceContext)
        {
        }

        public int InsertAudioFiles(AudioFiles audioFiles)
        {
            _serviceContext.AudioFiles.Add(audioFiles);
            _serviceContext.SaveChanges();
            return audioFiles.Id_AudioFiles;
        }

        public void UpdateAudioFiles(int Id_AudioFiles, AudioFiles updatedAudioFiles)
        {
            var existingAudioFiles = _serviceContext.AudioFiles.FirstOrDefault(p => p.Id_AudioFiles == Id_AudioFiles);

            if (existingAudioFiles == null)
            {
                // Si el producto no existe, podrías lanzar una excepción o manejar el caso según tus requerimientos.
                throw new InvalidOperationException("El AudioDile no existe.");
            }

            // Actualiza las propiedades del producto con la información del producto modificado
            existingAudioFiles.Title = updatedAudioFiles.Title;
            existingAudioFiles.Video = updatedAudioFiles.Video;
            existingAudioFiles.Description = updatedAudioFiles.Description;
            existingAudioFiles.Audio = updatedAudioFiles.Audio;
            existingAudioFiles.Id_category = updatedAudioFiles.Id_category;

            _serviceContext.SaveChanges();
        }

        public void DeleteAudioFile(int Id_AudioFiles)
        {
            var audioFile = _serviceContext.AudioFiles.Find(Id_AudioFiles);
            if (audioFile != null)
            {
                _serviceContext.AudioFiles.Remove(audioFile);
                _serviceContext.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("El Audio File no existe.");
            }
        }
    }
}
