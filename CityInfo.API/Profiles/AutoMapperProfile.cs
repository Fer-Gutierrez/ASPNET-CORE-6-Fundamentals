using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;

namespace CityInfo.API.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<City, CityWithoutPointOfInterestDto>().ReverseMap();
            CreateMap<PointOfInterest, PointsOfInterestDto>().ReverseMap();
            CreateMap<PointOfInterest, PointOfInterestCreationDto>().ReverseMap();
            CreateMap<PointOfInterestUpdateDto, PointOfInterest>().ReverseMap();
            
        }
    }
}
