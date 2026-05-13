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
        ValidateNewService(dto.Title, dto.Description, dto.Price);

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

    public GetServiceDto UpdateService(Guid id, UpdateServiceDto dto)
    {
        ValidateNewService(dto.Title, dto.Description, dto.Price);

        var service = uow.Services.GetById(id);

        if (service == null)
        {
            throw new NotFoundException("Сервіс не знайдено");
        }

        service.Title = dto.Title;
        service.Description = dto.Description;
        service.Price = dto.Price;
        
        uow.Services.Update(service);
        uow.Save();
        
        return mapper.Map<GetServiceDto>(service);   
    }

    public void DeleteService(Guid id)
    {
        var service = uow.Services.GetById(id);

        if (service == null)
        {
            throw new NotFoundException("Послугу не знайдено!");
        }

        if (uow.Orders.GetAll().Any(o => !o.IsDone && o.OrderServices.Any(s => s.ServiceId == id)))
        {
            throw new BadRequestException("Існують невиконані замовлення, що містять цю послугу!");
        }

        var orders = uow.Orders.GetAll();

        foreach (var order in orders)
        {
            if (order.OrderServices.Any(s => s.ServiceId == id))
            {
                uow.Orders.Delete(order);
            }
        }
        
        var packagesServices = uow.PackageServices.GetAll();

        foreach (var packageService in packagesServices)
        {
            if (packageService.ServiceId == id)
            {
                uow.PackageServices.Delete(packageService);
            }           
        }
        
        uow.Services.Delete(service);
        uow.Save();
    }

    private void ValidateNewService(string title, string description, decimal price)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new BadRequestException("Назва не може бути порожньою");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new BadRequestException("Опис не може бути порожнім");
        }

        if (price < 0)
        {
            throw new BadRequestException("Ціна не може бути від'ємною");
        }
    }
}