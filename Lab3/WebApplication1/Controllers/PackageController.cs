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
    }
}
