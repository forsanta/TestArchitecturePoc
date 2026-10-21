using TestArchitecturePoc.Application.Abstractions;
using TestArchitecturePoc.Domain.Entities;

namespace TestArchitecturePoc.Application.Services;

public sealed class OrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public IReadOnlyCollection<Order> GetAllOrders()
    {
        return _orderRepository.GetAll();
    }
}
