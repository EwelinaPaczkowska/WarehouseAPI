using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WarehouseAPI.DTOs;
using WarehouseAPI.Exceptions;
using WarehouseAPI.Repositories;

namespace WarehouseAPI.Services;

[ApiController]
[Route("api/[controller]")]
public class WarehouseService: IWarehouseService
{
    private readonly IWarehouseRepository _wareHouseRepository;

    public WarehouseService(IWarehouseRepository wareHouseRepository)
    {
        _wareHouseRepository = wareHouseRepository;
    }

    public async Task<int> AddProductAsync(ProductWarehouseDTO product, CancellationToken cancellationToken)
    {
        
        if(! await _wareHouseRepository.DoesProductExistAsync(product.IdProduct,cancellationToken))
            throw new NotFoundException("Produkt nie istnieje");
        
        if(! await _wareHouseRepository.DoesWareHouseExistAsync(product.IdWarehouse,cancellationToken))
            throw new NotFoundException("Magazyn nie istnieje");
        
        var orderid = await _wareHouseRepository.GetOrderIdAsync(product.IdProduct,product.Amount,DateTime.Now,cancellationToken);
        
        if(orderid<=0)
            throw new NotFoundException("Zamowienie nie istnieje");
        
        if(await _wareHouseRepository.DoesOrderExistAsync(orderid, cancellationToken))
            throw new ConflictException("Zamowienie jest w trakcie");
        
        var argument = new ProductWarehouseDTO()
        {
            IdProduct = product.IdProduct,
            IdWarehouse = product.IdWarehouse,
            Amount = product.Amount,
            CreatedAt = DateTime.Now
        };
        var response = await _wareHouseRepository.AddProductAsync(argument, cancellationToken);
        return response;
    }
}