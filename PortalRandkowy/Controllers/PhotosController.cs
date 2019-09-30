using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PortalRandkowy.Data;
using PortalRandkowy.Dtos;
using PortalRandkowy.Helpers;
using PortalRandkowy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PortalRandkowy.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IOptions<CloudinarySettings> cloudinaryConfig;
        private Cloudinary cloudinary;

        public PhotosController(IUserRepository userRepository, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.cloudinaryConfig = cloudinaryConfig;

            Account account = new Account(
                cloudinaryConfig.Value.CloudName,
                cloudinaryConfig.Value.APIKey,
                cloudinaryConfig.Value.APISecret
            );

            cloudinary = new Cloudinary(account);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var userFromRepo = await userRepository.GetUser(userId);

            var file = photoForCreationDto.File;
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = mapper.Map<Photo>(photoForCreationDto);

            if (!userFromRepo.Photos.Any(p => p.IsMain))
            {
                photo.IsMain = true;
            }

            userFromRepo.Photos.Add(photo);

            if (await userRepository.SaveAll())
            {
                var photoToReturn = mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new { id = photo.PhotoId }, photoToReturn);
            }

            return BadRequest("Nie można dodać zdjęcia");
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await userRepository.GetPhoto(id);

            var photoForReturn = mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photoForReturn);
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var user = await userRepository.GetUser(userId);

            if (!user.Photos.Any(p => p.PhotoId == id))
            {
                return Unauthorized();
            }

            var photoFromRepo = await userRepository.GetPhoto(id);

            if (photoFromRepo.IsMain)
            {
                return BadRequest("To już jest główne zdjęcie");
            }

            var currentMainPhoto = await userRepository.GetMainPhotoForUser(userId);
            currentMainPhoto.IsMain = false;
            photoFromRepo.IsMain = true;

            if (await userRepository.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("Nie można ustawić zdjęcia jako głównego");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var user = await userRepository.GetUser(userId);

            if (!user.Photos.Any(p => p.PhotoId == id))
            {
                return Unauthorized();
            }

            var photoFromRepo = await userRepository.GetPhoto(id);

            if (photoFromRepo.IsMain)
            {
                return BadRequest("Nie można usunąć zdjęcia głównego");
            }

            if (photoFromRepo.public_id != null)
            {
                var deleteParams = new DeletionParams(photoFromRepo.public_id);
                var result = cloudinary.Destroy(deleteParams);

                if (result.Result == "ok")
                {
                    userRepository.Delete(photoFromRepo);
                }
            }
            else
            {
                userRepository.Delete(photoFromRepo);
            }

            if (await userRepository.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Nie udało się usunąć zdjęcia");
        }
    }
}
