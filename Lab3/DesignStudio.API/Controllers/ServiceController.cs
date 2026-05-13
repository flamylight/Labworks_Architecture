using Contracts.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpPost]
        public ActionResult<GetServiceDto> CreateService([FromBody] CreateServiceDto dto)
        {
            var newService = serviceManager.CreateService(dto);
            
            return Ok(newService);
        }

        [HttpGet]
        public ActionResult<IEnumerable<GetServiceDto>> GetAllServices()
        {
            var services = serviceManager.GetAllServices();
            
            return Ok(services);
        }

        [HttpPut("{id}")]
        public ActionResult<GetServiceDto> UpdateService([FromRoute] Guid id, [FromBody] UpdateServiceDto dto)
        {
            var updatedService = serviceManager.UpdateService(id, dto);
            
            return Ok(updatedService);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteService([FromRoute] Guid id)
        {
            serviceManager.DeleteService(id);
            
            return NoContent();
        }
    }
}
