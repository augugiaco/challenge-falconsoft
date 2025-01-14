using FalconSoftChallenge.Business.DTO;
using FalconSoftChallenge.Business.Interfaces;
using FalconSoftChallenge.Business.QueryObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FalconSoftChallenge.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] string? description, 
            [FromQuery] decimal? amount,
            [FromQuery] DateTime? createdDate,
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10, 
            [FromQuery] OrderSortingField sortingField = OrderSortingField.CreatedDate, 
            [FromQuery] OrderSortingWay sortingWay = OrderSortingWay.Desc) 
        {
            var result = await _orderService.GetPaginated(
                new OrdersQueryModel(
                    description, 
                    amount, 
                    createdDate, 
                    page, 
                    pageSize, 
                    sortingField, 
                    sortingWay));

            return Ok(result);
        }

        [HttpPatch]
        [Route("{orderId}")]
        public async Task<IActionResult> Update(Guid orderId, [FromBody] UpdateOrderDTO updateOrderDTO) 
        {
            updateOrderDTO.OrderId = orderId;

            var orderUpdated = await _orderService.Update(updateOrderDTO);

            return Ok(orderUpdated);
        }
    }
}
