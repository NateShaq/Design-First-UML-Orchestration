using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmnichannelRetail.Api.Data;
using OmnichannelRetail.Api.Models;

namespace OmnichannelRetail.Api.Controllers;

[ApiController]
[Route("api/inventory")]
public class InventoryController : ControllerBase
{
    private readonly RetailDbContext _context;
    public InventoryController(RetailDbContext context)
    {
        _context = context;
    }

    [HttpGet("{inventoryId:guid}")]
    public async Task<ActionResult<Inventory>> Get(Guid inventoryId)
    {
        var inventory = await _context.Inventories.Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.InventoryId == inventoryId);
        if (inventory == null) return NotFound();
        return inventory;
    }

    [HttpPost]
    public async Task<ActionResult<Inventory>> Create(Inventory inventory)
    {
        _context.Inventories.Add(inventory);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { inventoryId = inventory.InventoryId }, inventory);
    }

    [HttpPost("{inventoryItemId:guid}/reserve")]
    public async Task<IActionResult> Reserve(Guid inventoryItemId, [FromQuery] int quantity, [FromHeader(Name="If-Match")] string? rowVersion)
    {
        var item = await _context.InventoryItems.Include(ii => ii.Inventory)
            .FirstOrDefaultAsync(ii => ii.InventoryItemId == inventoryItemId);
        if (item == null) return NotFound();

        if (rowVersion != null)
        {
            _context.Entry(item).Property("RowVersion").OriginalValue = Convert.FromBase64String(rowVersion);
        }

        if (item.QuantityOnHand - item.Reserved < quantity)
        {
            return Conflict("Insufficient quantity.");
        }

        item.Reserved += quantity;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict("Concurrency violation detected (ghost write protection).");
        }

        return NoContent();
    }
}
