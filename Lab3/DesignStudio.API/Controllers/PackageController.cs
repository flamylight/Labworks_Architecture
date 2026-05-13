using Contracts.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController(IPackageManager packageManager) : ControllerBase
    {
        [HttpPost]
        public ActionResult<GetPackageDto> CreatePackage([FromBody] CreatePackageDto dto)
        {
            var newPackage = packageManager.CreatePackage(dto);
            
            return Ok(newPackage);
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<GetPackageDto>> GetAllPackages()
        {
            var packages = packageManager.GetAllPackages();
            
            return Ok(packages);
        }

        [HttpPut("{packageId}/services/{serviceId}")]
        public ActionResult AddServiceToPackage([FromRoute] Guid packageId, [FromRoute] Guid serviceId)
        {
            packageManager.AddServiceToPackage(packageId, serviceId);
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePackage([FromRoute] Guid id)
        {
            packageManager.DeletePackage(id);
            
            return NoContent();       
        }
    }
}
