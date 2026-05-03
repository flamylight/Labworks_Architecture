using BLL.Exceptions;
using Contracts.DTOs;
using BLL.Interfaces;
using BLL.Mappers;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Managers;

public class PackageManager(IUnitOfWork uow) : IPackageManager
{
    public GetPackageDto CreatePackage(CreatePackageDto dto)
    {
        ValidateNewPackage(dto);
        
        var package = dto.ToEntity();
        
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

                package.TotalPrice += service.Price;
            }
            else
            {
                throw new NotFoundException("Сервіс не знайдено");
            }
        }
        
        uow.Packages.Add(package);
        uow.Save();
        
        return package.ToGetDto();
    }

    public IEnumerable<GetPackageDto> GetAllPackages()
    {
        return uow.Packages.GetAll().Select(p => p.ToGetDto()).ToList();
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