using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers;

public class CategoryController : BaseController
{
    public CategoryController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> get()
    {
        try
        {
            var Categorys = await _unitOfWork.CategoryRepositry.GetAllAsync();
            if (Categorys is null)
                return BadRequest(new ResponseAPI(400));
            return Ok(Categorys);
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
            var category = await _unitOfWork.CategoryRepositry.GetByIdAsync(id);
            if (category is null) return BadRequest(new ResponseAPI(400, $"Not Found Category ID={id}"));
            return Ok(category);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("add-category")]
    public async Task<IActionResult> add(CategoryDTO categoryDTO)
    {
        try
        {
            await _unitOfWork.CategoryRepositry.AddAsync(_mapper.Map<Category>(categoryDTO));
            return Ok(new ResponseAPI(200, "Item Has Been Added"));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut("update-category")]
    public async Task<IActionResult> update(UpdateCategoryDTO updateCategoryDTO)
    {
        try
        {
            await _unitOfWork.CategoryRepositry.UpdateAsync(_mapper.Map<Category>(updateCategoryDTO));
            return Ok(new ResponseAPI(200, "Item Has Been Update"));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpDelete("delete-category/{id}")]
    public async Task<IActionResult> delete(int id)
    {
        try
        {
            await _unitOfWork.CategoryRepositry.DeleteAsync(id);
            return Ok(new ResponseAPI(200, "item has been deleted"));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}