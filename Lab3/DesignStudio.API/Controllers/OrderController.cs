using Contracts.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderManager orderManager) : ControllerBase
    {
        [HttpPost("service")]
        public ActionResult<GetOrderDto> CreateServiceOrder([FromBody] CreateOrderDto dto)
        {
            var newOrder = orderManager.CreateServiceOrder(dto);
            
            return Ok(newOrder);
        }

        [HttpPost("turnkey")]
        public ActionResult<GetOrderDto> CreateTurnkeyOrder([FromBody] CreateTurnkeyOrderDto dto)
        {
            var newOrder = orderManager.CreateTurnkeyOrder(dto);
            
            return Ok(newOrder);
        }

        [HttpGet]
        public ActionResult<IEnumerable<GetOrderDto>> GetAllOrders()
        {
            var orders = orderManager.GetAllOrders();
            
            return Ok(orders);
        }
        
        [HttpPut("{id}/done")]
        public ActionResult MarkAsDone(Guid id)
        {
            orderManager.MarkAsDone(id);
            
            return NoContent();
        }
        
        [HttpGet("portfolio")]
        public ActionResult<IEnumerable<GetOrderDto>> GetPortfolioOrders()
        {
            var orders = orderManager.GetPortfolioOrders();
            
            return Ok(orders);
        }
        
        [HttpGet("done")]
        public ActionResult<IEnumerable<GetOrderDto>> GetDoneOrders()
        {
            var orders = orderManager.GetDoneOrders();
            
            return Ok(orders);
        }
        
        [HttpPut("{id}/portfolio")]
        public ActionResult MarkAsPortfolio(Guid id)
        {
            orderManager.MarkAsPortfolio(id);
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteOrder([FromRoute] Guid id)
        {
            orderManager.DeleteOrder(id);
            
            return NoContent();
        }
    }
}
