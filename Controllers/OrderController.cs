using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseAPI.Data;
using WarehouseAPI.Models;

namespace WarehouseAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrdesController(AppDbContext context) : ControllerBase
{
    // 1. GET: api/v1/orders
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        return await context.Orders.ToListAsync();
    }

    // 2. GET: api/v1/products/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await context.Orders.FindAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        return order;
    }

    // 3. POST: api/v1/orders
    [HttpPost]
    public async Task<ActionResult<Order>> PostProduct(Order order)
    {
        context.Orders.Add(order);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }
}
