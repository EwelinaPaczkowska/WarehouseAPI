﻿namespace WarehouseAPI.DTOs;

public class ProductWarehouseDTO
{
    public int IdProduct { get; set; }
    public int IdWarehouse { get; set; }
    public int Amount { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
}