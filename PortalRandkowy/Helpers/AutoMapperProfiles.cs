using AutoMapper;
using PortalRandkowy.Dtos;
using PortalRandkowy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalRandkowy.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl, opt => { opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url); })
                .ForMember(dest => dest.Age, opt => { opt.ResolveUsing(src => src.DateOfBirth.CalculateAge()); });
            
            CreateMap<User, UserForDetailsDto>()
                .ForMember(dest => dest.PhotoUrl, opt => { opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url); })
                .ForMember(dest => dest.Age, opt => { opt.ResolveUsing(src => src.DateOfBirth.CalculateAge()); });

            CreateMap<Photo, PhotosForDetailDto>();

            CreateMap<UserForUpdateDto, User>();

            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();

            CreateMap<UserForRegisterDto, User>();
        }
    }
}
