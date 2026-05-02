using Contracts.DTOs;
using BLL.Interfaces;
using BLL.Mappers;
using DAL.Interfaces;

namespace BLL.Managers;

public class ServiceManager(IUnitOfWork uow) : IServiceManager
{
    public GetServiceDto CreateService(CreateServiceDto dto)
    {
        ValidateNewService(dto);

        var serviceEntity = dto.ToEntity();
        
        uow.Services.Add(serviceEntity);
        uow.Save();

        return serviceEntity.ToGetDto();
    }

    public IEnumerable<GetServiceDto> GetAllServices()
    {
        var services = uow.Services.GetAll();
        
        return services.Select(s => s.ToGetDto()).ToList();
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