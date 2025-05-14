using WarehouseAPI.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace WarehouseAPI.Services
{
    public interface IWarehouseService
    {
        Task<int> AddProductUsingStoredProcedureAsync(ProductWarehouseDTO dto);
    }
}