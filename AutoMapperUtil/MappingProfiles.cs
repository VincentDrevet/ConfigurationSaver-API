using AutoMapper;
using Models;
using Dto;
using ConfigurationSaver_API.Dto;
using ConfigurationSaver_API.Models;

namespace AutoMapperUtil
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Credential, CredentialDto>();
            CreateMap<Device, DeviceDto>();
            CreateMap<ScheduleTask, ScheduleTaskDto>();
            CreateMap<CreateCredentialDto, Credential>();
            CreateMap<CreateDeviceDto, EsxiServer>();
            CreateMap<CreateDeviceDto, Device>();
            CreateMap<CreateScheduleTaskDto, ScheduleTask>();
            CreateMap<UpdateCredentialDto, Credential>();
            CreateMap<UpdateDeviceDto, Device>();
            CreateMap<UpdateScheduleTask, ScheduleTask>();
        }
    }
}