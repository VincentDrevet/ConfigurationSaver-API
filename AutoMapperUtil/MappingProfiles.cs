using AutoMapper;
using Models;
using Dto;
using ConfigurationSaver_API.Dto;

namespace AutoMapperUtil
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Credential, CredentialDto>();
            CreateMap<Server, ServerDto>();
            CreateMap<ScheduleTask, ScheduleTaskDto>();
            CreateMap<CreateCredentialDto, Credential>();
            CreateMap<CreateServerDto, Server>();
            CreateMap<CreateScheduleTaskDto, ScheduleTask>();
            CreateMap<UpdateCredentialDto, Credential>();
            CreateMap<UpdateServerDto, Server>();
            CreateMap<UpdateScheduleTask, ScheduleTask>();
        }
    }
}