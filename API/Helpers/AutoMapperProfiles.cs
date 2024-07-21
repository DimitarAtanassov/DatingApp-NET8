using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers;

// B/c we are deriving from Profile, we can have automapper profiles litered all over our code, 
// But aslong as they deriving from Profile then automapper, as soon as application starts up is going to register these profiles for anyhting that dervies from its Profile class
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        // CreateMap<MapFrom,MapTo>();
        // .ForMember(DestinationMember, WhereToMapFrom)
        // dest => dest.PhotoUrl: Specifies that the PhotoUrl property of the destination (MemberDto) is being configured.
        // o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain)!.Url): Specifies the custom mapping logic for the PhotoUrl property
        // s => s.Photos: Accesses the Photos collection of the source object (AppUser).
        // .FirstOrDefault(x => x.IsMain): Finds the first photo in the Photos collection where the IsMain property is true. This means it is looking for the main photo of the user.
        // !.Url: The null-forgiving operator ! is used to indicate that FirstOrDefault will not return null in this context. It ensures that the Url property is accessed safely.
        CreateMap<AppUser,MemberDto>()
            .ForMember(dest => dest.Age, o => o.MapFrom(s=> s.DateOfBirth.CalculateAge()))
            .ForMember(dest => dest.PhotoUrl, o => 
            o.MapFrom(s => s.Photos.FirstOrDefault(x=>x.IsMain)!.Url));
        CreateMap<Photo,PhotoDto>();

        // Workflow: first start with the basic create maps, and check for the mappings
        // If there is any gaps then we can come back and configure it to map the properties we are missing
    }

}
