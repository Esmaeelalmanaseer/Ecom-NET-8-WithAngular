using AutoMapper;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers;

public class BugController : BaseController
{
    public BugController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }
    [HttpGet("not-found")]
    public async Task<IActionResult> GetNotFound()
    {
        var category = await _unitOfWork.CategoryRepositry.GetByIdAsync(-99);
        if (category is null) return NotFound();
        return Ok(category);
    }
    [HttpGet("server-error")]
    public async Task<IActionResult> GetServerError()
    {
        var category = await _unitOfWork.CategoryRepositry.GetByIdAsync(-99);
        category.Name = "";
        return Ok(category);
    }
    [HttpGet("bad-request/{id}")]
    public async Task<IActionResult> getbadrequeset(int id)
    {
        return Ok();
    }
    [HttpGet("bad-request/")]
    public async Task<IActionResult> getbadrequest()
    {
        return BadRequest();
    }
}
