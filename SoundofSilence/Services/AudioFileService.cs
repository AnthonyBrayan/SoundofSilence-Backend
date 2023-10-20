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
          
            var category = _serviceContext.Category.FirstOrDefault(c => c.Id_category == audioFiles.Id_category);

            if (category == null)
            {
              
                throw new InvalidOperationException("The category is not valid.");
            }

            
            audioFiles.Id_category = category.Id_category; 

            
            _serviceContext.AudioFiles.Add(audioFiles);
            _serviceContext.SaveChanges();

            return audioFiles.Id_AudioFiles;
        }

        public void UpdateAudioFiles(int Id_AudioFiles, AudioFiles updatedAudioFiles)
        {
            var existingAudioFiles = _serviceContext.AudioFiles.FirstOrDefault(p => p.Id_AudioFiles == Id_AudioFiles);

            if (existingAudioFiles == null)
            {
               
                throw new InvalidOperationException("AudioDile does not exist.");
            }

           
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
                throw new InvalidOperationException("AudioDile does not exist.");
            }
        }

        public List<AudioFiles> GetAudioFiles()
        {
            return _serviceContext.AudioFiles.ToList();
        }

        public bool Exists(int Id_AudioFiles)
        {
            return _serviceContext.AudioFiles.Any(a => a.Id_AudioFiles == Id_AudioFiles);

    }
        public List<AudioFiles> GetAudioFilesByCategory(int Id_category)
        {
            
            return _serviceContext.AudioFiles.Where(audioFile => audioFile.Id_category == Id_category).ToList();

        }
        }
    }
