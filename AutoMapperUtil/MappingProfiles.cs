using AutoMapper;
using Models;
using Dto;

namespace AutoMapperUtil
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Credential, CredentialDto>();
            CreateMap<Server, ServerDto>();
            CreateMap<ScheduleTask, ScheduleTaskDto>();
        }
    }
}