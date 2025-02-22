using Microsoft.AspNetCore.Http;

namespace Ecom.Core.DTO;

public record ProductDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public virtual List<PhotoDTO>Photos { get; set; }
    public string CategoryName { get; set; }
}

public record PhotoDTO
{
    public string ImageName { get; set; }
    public int ProductId { get; set; }
}

public record AddProductDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
    public int CategoryId { get; set; }
    public IFormFileCollection Photo { get; set; }
}

