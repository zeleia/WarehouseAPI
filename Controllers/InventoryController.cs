using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseAPI.Data;
using WarehouseAPI.Models;

namespace WarehouseAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class InventoryController(AppDbContext context) : ControllerBase
{
   // 1. GET: api/v1/inventory/low-stock
    [HttpGet("low.stock")]
    public async Task<ActionResult<IEnumerable<Product>>> GetLowStockProducts(int threshold = 10)
    {
        return await context.Products.Where(p => p.StockQuantity <= threshold).ToListAsync();
    }

    // 2. GET: api/v1/inventory
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryAuditLog>>> GetInventoryAuditLogs()
    {
        return await context.InventoryAuditLogs.ToListAsync();
    }

    //3. POST: api/v1/inventory
    [HttpPut("{id}")]
    public async Task<IActionResult> PostInventoryAuditLog(int id, InventoryAuditLog log)
    {
        context.InventoryAuditLogs.Add(log);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetInventoryAuditLogs), new { id = log.Id }, log);
    }
}
