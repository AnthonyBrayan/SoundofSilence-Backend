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
            // Verifica si el Id_category proporcionado es válido
            var category = _serviceContext.Category.FirstOrDefault(c => c.Id_category == audioFiles.Id_category);

            if (category == null)
            {
                // Si el Id_category no es válido, podrías lanzar una excepción o manejar el caso según tus requerimientos.
                throw new InvalidOperationException("La categoría no es válida.");
            }

            // Establece la categoría del audioFiles
            audioFiles.Id_category = category.Id_category; // Establecer el ID de la categoría

            // Agrega el audioFiles al contexto y guarda los cambios
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
            existingAudioFiles.title = updatedAudioFiles.title;
            existingAudioFiles.videoSrc = updatedAudioFiles.videoSrc;
            existingAudioFiles.description = updatedAudioFiles.description;
            existingAudioFiles.audioSrc = updatedAudioFiles.audioSrc;
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

        public List<AudioFiles> GetAudioFiles()
        {
            return _serviceContext.AudioFiles.ToList();
        }


        public List<AudioFiles> GetAudioFilesByCategory(int Id_category)
        {
            // Filtra los archivos de audio por Id_category
            return _serviceContext.AudioFiles.Where(audioFile => audioFile.Id_category == Id_category).ToList();
        }


    }
}
