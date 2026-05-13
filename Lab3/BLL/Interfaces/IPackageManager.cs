using Contracts.DTOs;

namespace BLL.Interfaces;

public interface IPackageManager
{
    GetPackageDto CreatePackage(CreatePackageDto dto);
    IEnumerable<GetPackageDto> GetAllPackages();
    void AddServiceToPackage(Guid packageId, Guid serviceId);
    void DeletePackage(Guid packageId);
}