using BLL.DTOs;

namespace BLL.Interfaces;

public interface IServiceManager
{
    GetServiceDto CreateService(CreateServiceDto dto);
    IEnumerable<GetServiceDto> GetAllServices();
}