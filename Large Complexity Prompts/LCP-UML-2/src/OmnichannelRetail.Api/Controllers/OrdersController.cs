using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmnichannelRetail.Api.Data;
using OmnichannelRetail.Api.Dto;
using OmnichannelRetail.Api.Models;

namespace OmnichannelRetail.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly RetailDbContext _context;
    public OrdersController(RetailDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<Order>> Create(CreateOrderDto dto)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        if (await _context.Orders.AnyAsync(o => o.OrderNumber == dto.OrderNumber))
        {
            return Conflict($"Order {dto.OrderNumber} already exists.");
        }

        var order = new Order
        {
            OrderNumber = dto.OrderNumber,
            Status = dto.Status,
            CustomerId = dto.CustomerId,
            Total = dto.Total,
            IsolationLevel = IsolationLevel.Serializable.ToString()
        };

        foreach (var line in dto.Lines)
        {
            order.Lines.Add(new OrderLine
            {
                OrderNumber = dto.OrderNumber,
                ProductSku = line.ProductSku,
                Quantity = line.Quantity,
                UnitPrice = line.UnitPrice
            });
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        await tx.CommitAsync();

        return CreatedAtAction(nameof(GetByNumber), new { orderNumber = order.OrderNumber }, order);
    }

    [HttpGet("{orderNumber}")]
    public async Task<ActionResult<Order>> GetByNumber(string orderNumber)
    {
        var order = await _context.Orders.Include(o => o.Lines)
                                         .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
        if (order == null) return NotFound();
        return order;
    }
}
