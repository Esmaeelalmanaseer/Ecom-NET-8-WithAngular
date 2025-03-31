using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Ecom.Infrastructure.Repositories;
public class CustomerBasketRepositry : ICustomerBasketRepositry
{
    private readonly IDatabase _database;
    public CustomerBasketRepositry(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }
    public async Task<bool> DeleteBasketAsync(string id)
    {
        return await _database.KeyDeleteAsync(id);
    }

    public async Task<CustomerBasket?> GetBaskerAsync(string id)
    {
        var result = await _database.StringGetAsync(id);
        if (!string.IsNullOrEmpty(result))
            return JsonSerializer.Deserialize<CustomerBasket>(result!)!;
        return null;
    }

    public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket CustomerBasketobj)
    {
        var basket = await _database.StringSetAsync(CustomerBasketobj.Id, JsonSerializer.Serialize<CustomerBasket>(CustomerBasketobj),TimeSpan.FromDays(3));
        if (basket)
            return await GetBaskerAsync(CustomerBasketobj.Id);
        return null;
    }
}