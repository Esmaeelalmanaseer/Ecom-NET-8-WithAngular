using Ecom.Core.Entities;

namespace Ecom.Core.Interfaces;
public interface ICustomerBasketRepositry
{
    Task<CustomerBasket?> GetBaskerAsync(string id);
    Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket CustomerBasketobj);
    Task<bool> DeleteBasketAsync(string id);
}