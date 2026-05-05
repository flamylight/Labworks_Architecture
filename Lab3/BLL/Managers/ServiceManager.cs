using AutoMapper;
using BLL.Exceptions;
using Contracts.DTOs;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Managers;

public class ServiceManager(IUnitOfWork uow, IMapper mapper) : IServiceManager
{
    public GetServiceDto CreateService(CreateServiceDto dto)
    {
        ValidateNewService(dto);

        var serviceEntity = mapper.Map<Service>(dto);
        
        uow.Services.Add(serviceEntity);
        uow.Save();

        return mapper.Map<GetServiceDto>(serviceEntity);
    }

    public IEnumerable<GetServiceDto> GetAllServices()
    {
        var services = uow.Services.GetAll();
        
        return services.Select(s => mapper.Map<GetServiceDto>(s)).ToList();
    }

    private void ValidateNewService(CreateServiceDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new BadRequestException("Назва не може бути порожньою");
        }

        if (string.IsNullOrWhiteSpace(dto.Description))
        {
            throw new BadRequestException("Опис не може бути порожнім");
        }

        if (dto.Price < 0)
        {
            throw new BadRequestException("Ціна не може бути від'ємною");
        }
    }
}