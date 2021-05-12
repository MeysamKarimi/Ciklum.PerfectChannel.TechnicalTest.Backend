using AutoMapper;

namespace PerfectChannel.WebApi.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Data.Models.Task, Services.DTOs.Task>();
            CreateMap<Services.DTOs.Task, Data.Models.Task>();
        }
    }
}