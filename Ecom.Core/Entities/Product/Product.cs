﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Ecom.Core.Entities.Product;

public class Product : BaseEntity<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public virtual Category Category { get; set; }
    public ICollection<Photo> Photos { get; set; } = new HashSet<Photo>();
}