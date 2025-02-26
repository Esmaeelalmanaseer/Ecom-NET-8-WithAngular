using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers;

public class ProductController : BaseController
{
    public ProductController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }
    [HttpGet("get-all")]
    public async Task<IActionResult> get()
    {
        try
        {
            var products = await _unitOfWork.ProductRepositry.GetAllAsync(x => x.Category, x => x.Photos);
            if (products is null) return BadRequest(new ResponseAPI(400));
            return Ok(_mapper.Map<List<ProductDTO>>(products));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("get-by-id/{id}")]
    public async Task<IActionResult> getbyid(int id)
    {
        try
        {
            var product = await _unitOfWork.ProductRepositry.GetByIdAsync(id, x => x.Category, x => x.Photos);
            if (product is null) return BadRequest(new ResponseAPI(400));
            return Ok(_mapper.Map<ProductDTO>(product));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("Add-Product")]
    public async Task<IActionResult> add(AddProductDTO addProductDTO)
    {
        try
        {
            var result = await _unitOfWork.ProductRepositry.AddAsync(addProductDTO);
            if (result)
                return Ok(new ResponseAPI(200, $"Add Product Susccfuly"));
            return BadRequest(new ResponseAPI(400));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut("update-product")]
    public async Task<IActionResult>update(UpdateProductDTO UpdateProduct)
    {
        try
        {
            var result=await _unitOfWork.ProductRepositry.UpdateAsync(UpdateProduct);
            if (result) return Ok(new ResponseAPI(200,"Product Has Been Updated"));
            return BadRequest(new ResponseAPI(400));
                
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpDelete("Delete-Product/{id}")]
    public async Task<IActionResult>delete(int id)
    {
        try
        {
            var product = await _unitOfWork.ProductRepositry.GetByIdAsync(id, x => x.Category, x => x.Photos);
            await _unitOfWork.ProductRepositry.DeleteAsync(product);
            return Ok(new ResponseAPI(200));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
