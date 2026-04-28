using BLL.DTOs;
using DAL.Models;

namespace BLL.Mappers;

public static class PackageMapper
{
    public static Package ToEntity(this CreatePackageDto dto)
    {
        return new Package
        {
            Title = dto.Title,
            Description = dto.Description,
        };
    }

    public static GetPackageDto ToGetDto(this Package entity)
    {
        return new GetPackageDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            TotalPrice = entity.TotalPrice,
            PackageServicesItemDto = entity.PackageServices.Select(ps => new PackageServiceItemDto
            {
                Title = ps.Service?.Title ?? "Видалений сервіс",
                Price = ps.Service?.Price ?? 0
            }).ToList()
        };   
    }
}