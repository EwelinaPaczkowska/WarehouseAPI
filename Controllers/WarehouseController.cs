using Microsoft.AspNetCore.Mvc;
using WarehouseAPI.DTOs;
using WarehouseAPI.Services;

namespace WarehouseAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController: ControllerBase
{
    private readonly IWarehouseService _service;
    public WarehouseController(IWarehouseService service)
    {
        _service = service;
    }

    [HttpPut]
    public async Task<IActionResult> AddProductAsync(ProductWarehouseDTO product,CancellationToken cancellationToken)
    {
        var response = await _service.AddProductAsync(product, cancellationToken);
        return Ok(response);
    }
}