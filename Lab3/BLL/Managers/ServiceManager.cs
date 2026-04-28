using BLL.DTOs;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Managers;

public class ServiceManager(IUnitOfWork uow) : IServiceManager
{
    public GetServiceDto CreateService(CreateServiceDto dto)
    {
        ValidateNewService(dto);
        
        var serviceEntity = new Service
        {
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price
        };
        
        uow.Services.Add(serviceEntity);
        uow.Save();

        return new GetServiceDto
        {
            Id = serviceEntity.Id,
            Title = serviceEntity.Title,
            Description = serviceEntity.Description,
            Price = serviceEntity.Price
        };
    }

    private void ValidateNewService(CreateServiceDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new ArgumentException("Назва не може бути порожньою");
        }

        if (string.IsNullOrWhiteSpace(dto.Description))
        {
            throw new ArgumentException("Опис не може бути порожнім");
        }

        if (dto.Price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(dto.Price), "Ціна не може бути від'ємною");
        }
    }
}