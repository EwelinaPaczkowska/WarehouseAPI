using System.Data;
using Microsoft.Data.SqlClient;
using WarehouseAPI.DTOs;

namespace WarehouseAPI.Procedures;

public class ProcedureExecutor
{
    private readonly string _connectionString;

    public ProcedureExecutor(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<int> Execute(ProductWarehouseDTO dto)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("AddProductToWarehouse", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@IdProduct", dto.IdProduct);
        cmd.Parameters.AddWithValue("@IdWarehouse", dto.IdWarehouse);
        cmd.Parameters.AddWithValue("@Amount", dto.Amount);
        cmd.Parameters.AddWithValue("@CreatedAt", dto.CreatedAt);

        await conn.OpenAsync();
        var result = await cmd.ExecuteScalarAsync();

        return Convert.ToInt32(result);
    }
}