﻿using Data;
using Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SoundofSilence.IServices;
using SoundofSilence.Models;
using SoundofSilence.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Web.Http.Cors;

namespace SoundofSilence.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    

    [Route("[controller]/[action]")]
    public class AudioFilesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAudioFilesService _audioFilesService;
        private readonly ServiceContext _serviceContext;

        public AudioFilesController(IConfiguration configuration, IAudioFilesService audioFilesService, ServiceContext serviceContext)
        {
            _configuration = configuration;
            _audioFilesService = audioFilesService;
            _serviceContext = serviceContext;
        }

        [HttpPost(Name = "InsertAudioFiles")]
        public IActionResult Post([FromBody] AudioFiles audioFiles)
        {
            try
            {
                return Ok(_audioFilesService.InsertAudioFiles(audioFiles));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting role ID: {ex.Message}");
            }

            
        }

        [HttpPatch("{Id_AudioFiles}", Name = "UpdateAudioFiles")]
        public IActionResult Put([FromRoute]  int Id_AudioFiles, [FromBody] AudioFiles updatedAudioFiles)
        {
            _audioFilesService.UpdateAudioFiles(Id_AudioFiles, updatedAudioFiles);
            return NoContent();
        }

        [HttpDelete("{Id_AudioFile}", Name = "DeleteAudioFile")]
        public IActionResult Delete([FromRoute] int Id_AudioFile)
        {
            _audioFilesService.DeleteAudioFile(Id_AudioFile);

           
            return Ok(new { message = "AudioFile successfully deleted" });
        }

        [HttpGet(Name = "GetAudioFiles")]
        public List<AudioFiles> Get()
        {

            return _audioFilesService.GetAudioFiles();
        }


        [HttpGet("{Id_category}", Name = "GetAudioFilesByCategory")]
        public List<AudioFiles> GetByCategory([FromRoute] int Id_category)
        {
            return _audioFilesService.GetAudioFilesByCategory(Id_category);
        }

    }
}
