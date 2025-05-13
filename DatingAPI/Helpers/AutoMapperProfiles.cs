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
        CreateMap<MemberUpdateDto, appUser>();
        CreateMap<RegisterDto, appUser>();
        CreateMap<string, DateOnly>().ConstructUsing(s => DateOnly.Parse(s));
        CreateMap<Message, MessageDto>()
            .ForMember(d => d.SenderPhotoUrl, o => o.MapFrom(s => s.Sender.Photos.FirstOrDefault(x => x.IsMain)!.Url))
            .ForMember(d => d.RecipientPhotoUrl, o => o.MapFrom(s => s.Recipient.Photos.FirstOrDefault(x => x.IsMain)!.Url));
    }

}
