using AutoMapper;
using Contracts.DTOs;
using DAL.Models;

namespace BLL.Mapping;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        CreateMap<Service, GetServiceDto>();
        
        CreateMap<CreateServiceDto, Service>();
    }
}