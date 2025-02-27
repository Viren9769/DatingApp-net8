using AutoMapper;
using DatingAPI.DTOs;
using DatingAPI.Entities;
using DatingAPI.Extensions;

namespace DatingAPI.Helpers;


public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles() 
    {
    CreateMap<appUser, MembersDTO >()
            .ForMember(d => d.Age, o => o.MapFrom(s => s.DateOfBirth.CalculateAge()))
            .ForMember(d => d.PhotoUrl, o => o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain)!.Url));
    CreateMap<Photo, PhotoDto>();
    }

}
