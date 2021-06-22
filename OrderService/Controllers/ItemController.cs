using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Database;
using OrderService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly Services.OrderService _orderService;

        public ItemController(Services.OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult GetItems()
        {
            return Ok(_orderService.GetItems());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(Guid id)
        {
            return Ok(await _orderService.GetItemByIdAsync(id));
        }
        
        [HttpPost("order")]
        public async Task<IActionResult> PlaceOrder(PlaceOrderDto dto)
        {
            await _orderService.PlaceOrderAsync(dto);

            return Ok();
        }
    }
}
