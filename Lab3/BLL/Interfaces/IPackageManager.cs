using BLL.DTOs;

namespace BLL.Interfaces;

public interface IPackageManager
{
    GetPackageDto CreatePackage(CreatePackageDto dto);
    IEnumerable<GetPackageDto> GetAllPackages();
}