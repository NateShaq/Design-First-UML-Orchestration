using LcpUml8.Api.Data;
using LcpUml8.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml8.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly LcpUml8Context _context;

    public ProductsController(LcpUml8Context context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll(CancellationToken cancellationToken)
    {
        var products = await _context.Products.AsNoTracking().ToListAsync(cancellationToken);
        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(ProductRequest request, CancellationToken cancellationToken)
    {
        var product = request.Type?.ToLowerInvariant() switch
        {
            "rawmaterial" => new RawMaterial(),
            "subassembly" => new SubAssembly(),
            "finishedproduct" => new FinishedProduct(),
            _ => null
        };

        if (product is null)
        {
            return BadRequest("Type must be RawMaterial, SubAssembly, or FinishedProduct.");
        }

        product.Name = request.Name;
        product.Sku = request.Sku;
        product.Version = request.Version;

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(string id, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(new object[] { id }, cancellationToken);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, ProductUpdate request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (product == null) return NotFound();

        if (request.RowVersion == null || request.RowVersion.Length == 0)
        {
            return BadRequest("RowVersion is required for optimistic concurrency.");
        }

        product.Name = request.Name;
        product.Sku = request.Sku;
        product.Version = request.Version;

        _context.Entry(product).Property(p => p.RowVersion).OriginalValue = request.RowVersion;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict("Concurrent update detected. Refresh the entity and retry.");
        }
    }
}

public record ProductRequest(string Type, string Name, string Sku, long Version);

public record ProductUpdate(string Name, string Sku, long Version, byte[] RowVersion);
