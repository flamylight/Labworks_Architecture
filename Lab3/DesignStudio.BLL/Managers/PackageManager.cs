using AutoMapper;
using BLL.Exceptions;
using Contracts.DTOs;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Managers;

public class PackageManager(IUnitOfWork uow, IMapper mapper) : IPackageManager
{
    public GetPackageDto CreatePackage(CreatePackageDto dto)
    {
        ValidateNewPackage(dto);
        
        var package = mapper.Map<Package>(dto);
        
        foreach (var serviceId in dto.Services.Distinct())
        {
            var service = uow.Services.GetById(serviceId);

            if (service != null)
            {
                package.PackageServices.Add(new PackageService
                {
                    PackageId = package.Id,
                    ServiceId = service.Id
                });
            }
            else
            {
                throw new NotFoundException("Сервіс не знайдено");
            }
        }
        
        uow.Packages.Add(package);
        uow.Save();
        
        return mapper.Map<GetPackageDto>(package);
    }

    public IEnumerable<GetPackageDto> GetAllPackages()
    {
        return uow.Packages.GetAll().Select(p => mapper.Map<GetPackageDto>(p)).ToList();
    }

    public void AddServiceToPackage(Guid packageId, Guid serviceId)
    {
        var package = uow.Packages.GetById(packageId);

        if (package == null)
        {
            throw new NotFoundException("Пакет не знайдено!");
        }
        
        var service = uow.Services.GetById(serviceId);

        if (service == null)
        {
            throw new BadRequestException("Послугу не знайдено!");
        }

        if (package.PackageServices.Any(ps => ps.ServiceId == serviceId))
        {
            throw new BadRequestException("Послуга вже є в пакеті!");
        }
        
        package.PackageServices.Add(new PackageService
        {
            ServiceId = serviceId,
            PackageId = packageId
        });
        
        uow.Packages.Update(package);
        uow.Save();       
    }

    public void DeletePackage(Guid packageId)
    {
        var package = uow.Packages.GetById(packageId);

        if (package == null)
        {
            throw new NotFoundException("Пакет не знайдено!");
        }
        
        uow.Packages.Delete(package);
        uow.Save();      
    }

    private void ValidateNewPackage(CreatePackageDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new BadRequestException("Назва не можу бути порожньою");
        }

        if (string.IsNullOrWhiteSpace(dto.Description))
        {
            throw new BadRequestException("Опис не може бути порожнім");
        }

        if (dto.Services == null || dto.Services.Count == 0)
        {
            throw new BadRequestException("Список сервісів не може бути порожнім");
        }
    }
}