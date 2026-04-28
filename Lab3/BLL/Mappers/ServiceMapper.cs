using BLL.DTOs;
using DAL.Models;

namespace BLL.Mappers;

public static class ServiceMapper
{
    public static GetServiceDto ToGetDto(this Service entity)
    {
        return new GetServiceDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            Price = entity.Price
        };
    }

    public static CreateServiceDto ToCreateDto(this Service entity)
    {
        return new CreateServiceDto
        {
            Title = entity.Title,
            Description = entity.Description,
            Price = entity.Price
        };
    }

    public static Service ToEntity(this CreateServiceDto dto)
    {
        return new Service
        {
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price
        };
    }

    public static Service ToEntity(this GetServiceDto dto)
    {
        return new Service
        {
            Id = dto.Id,
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price
        };
    }
}