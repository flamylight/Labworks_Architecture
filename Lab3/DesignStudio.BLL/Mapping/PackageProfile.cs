using AutoMapper;
using Contracts.DTOs;
using DAL.Models;

namespace BLL.Mapping;

public class PackageProfile : Profile
{
    public PackageProfile()
    {
        CreateMap<CreatePackageDto, Package>();

        CreateMap<PackageService, PackageServiceItemDto>()
            .ForMember(dest => dest.Title,
                opt =>
                    opt.MapFrom(src => src.Service != null
                        ? src.Service.Title
                        : "Послуга видалена"))
            .ForMember(dest => dest.Price,
            opt => 
                opt.MapFrom(src => src.Service != null 
                    ? src.Service.Price 
                    : 0));

        CreateMap<Package, GetPackageDto>()
            .ForMember(dest => dest.PackageServicesItemDto,
                opt =>
                    opt.MapFrom(src => src.PackageServices));
    }
}