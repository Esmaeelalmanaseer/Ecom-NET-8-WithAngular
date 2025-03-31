using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    public class BasketsController : BaseController
    {
        public BasketsController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        [HttpGet("get-basket-item/{id}")]
        public async Task<IActionResult>get(string id)
        {
            var result = await _unitOfWork.CustomerBasket.GetBaskerAsync(id);
            if (result is null)
                return BadRequest(new ResponseAPI(404));
            return Ok(result);
        }
        [HttpPost("update-basket")]
        public async Task<IActionResult>update(CustomerBasket customerBasketobj)
        {
            var basket=await _unitOfWork.CustomerBasket.UpdateBasketAsync(customerBasketobj);
            if (basket is null) return BadRequest(new ResponseAPI(400));
            return Ok(basket);
        }
        [HttpDelete("delete-basket-item/{id}")]
        public async Task<IActionResult>delete(string id)
        {
            var result=await _unitOfWork.CustomerBasket.DeleteBasketAsync(id);
            return result ? Ok(new ResponseAPI(200,"Item Deleted")) : BadRequest(new ResponseAPI(400));
        }
    }
}
