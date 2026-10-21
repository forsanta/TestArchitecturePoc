using Microsoft.AspNetCore.Mvc;
using TestArchitecturePoc.Application.Services;

namespace TestArchitecturePoc.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var orders = _orderService.GetAllOrders();
        return Ok(orders);
    }
}
