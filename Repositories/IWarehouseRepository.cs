using WarehouseAPI.DTOs;
using WarehouseAPI.Models;

namespace WarehouseAPI.Repositories;

public interface IWarehouseRepository
{
    Task<int> AddProductAsync(ProductWarehouseDTO product, CancellationToken cancellationToken);

    Task<bool> DoesProductExistAsync(int id, CancellationToken cancellationToken);
    
    Task<bool> DoesWareHouseExistAsync(int id, CancellationToken cancellationToken);
    
    Task<int> GetOrderIdAsync(int id,int amount,DateTime date, CancellationToken cancellationToken);
    
    Task<bool> DoesOrderExistAsync(int id, CancellationToken cancellationToken);
}