using Microsoft.Data.SqlClient;
using WarehouseAPI.DTOs;
using WarehouseAPI.Exceptions;

namespace WarehouseAPI.Repositories;

public class WarehouseRepository: IWarehouseRepository
{
    private readonly string _connectionString;
    
    public WarehouseRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    
    public async Task<int> AddProductAsync(ProductWarehouseDTO product, CancellationToken cancellationToken)
    {
        return 1;
    }
    
    public async Task<bool> DoesProductExistAsync(int id, CancellationToken cancellationToken)
    {
        
        await using (var connection = new SqlConnection(_connectionString)){
            
            await connection.OpenAsync(cancellationToken);

            var query = @"SELECT COUNT(1) FROM [dbo].[Product] WHERE IdProduct = @id";
            
            await using (var command = new SqlCommand(query, connection)){
                command.Parameters.AddWithValue("@id", id);
                
                var result = await command.ExecuteScalarAsync(cancellationToken);
                
                return Convert.ToInt32(result) > 0;
            }
        }
        
    }

    public async Task<bool> DoesWareHouseExistAsync(int id, CancellationToken cancellationToken)
    {
        await using (var connection = new SqlConnection(_connectionString)){
            
            await connection.OpenAsync(cancellationToken);

            var query = @"SELECT COUNT(1) FROM [dbo].[Warehouse] WHERE IdWarehouse = @id";
            
            await using (var command = new SqlCommand(query, connection)){
                command.Parameters.AddWithValue("@id", id);
                
                var result = await command.ExecuteScalarAsync(cancellationToken);
                
                return Convert.ToInt32(result) > 0;
            }
        }
    }

    public async Task<int> GetOrderIdAsync(int id,int amount,DateTime date, CancellationToken cancellationToken)
    {
        await using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);

            var query = @"SELECT 
                        [dbo].[Order].[IdOrder],
                        [dbo].[Order].[IdProduct],
                        [dbo].[Order].[Amount],
                        [dbo].[Order].[CreatedAt],
                        [dbo].[Order].[FulfilledAt]       
                        WHERE [dbo].[Order].[IdProduct] = @id";

            await using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                await using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    if(! reader.HasRows)
                        throw new NotFoundException("Order not found");

                    while (await reader.ReadAsync(cancellationToken))
                    {
                        var _amount = reader.GetInt32(reader.GetOrdinal("Amount"));
                        if(_amount != amount)
                            throw new BadRequestException("order with this id does not match amount");
                        
                        var _createdAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
                        if(date<_createdAt)
                            throw new ConflictException("current date is older than product date");
                        
                        var _fulfilledAt = reader.IsDBNull(reader.GetOrdinal("FulfilledAt"));
                        if(_fulfilledAt)
                            throw new ConflictException("order is fulfilled");
                        
                        var _orderId = reader.GetInt32(reader.GetOrdinal("IdOrder"));
                        return _orderId;
                    }
                    
                }
                
            }
            
        }
        return -1;
        
    }

    public async Task<bool> DoesOrderExistAsync(int id, CancellationToken cancellationToken)
    {
        await using (var connection = new SqlConnection(_connectionString)){
            
            await connection.OpenAsync(cancellationToken);

            var query = @"SELECT COUNT(1) FROM [dbo].[Product_Warehouse] WHERE IdOrder = @id";
            
            await using (var command = new SqlCommand(query, connection)){
                command.Parameters.AddWithValue("@id", id);
                
                var result = await command.ExecuteScalarAsync(cancellationToken);
                
                return Convert.ToInt32(result) > 0;
            }
        }
    }
}