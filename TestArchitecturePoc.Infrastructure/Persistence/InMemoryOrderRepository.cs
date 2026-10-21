using TestArchitecturePoc.Application.Abstractions;
using TestArchitecturePoc.Domain.Entities;

namespace TestArchitecturePoc.Infrastructure.Persistence;

public sealed class InMemoryOrderRepository : IOrderRepository
{
    private static readonly IReadOnlyCollection<Order> Orders =
    [
        new Order(Guid.Parse("9F397EE4-A5D8-4D35-B8F0-8BB0A4CC5038"), "Alice", 120m),
        new Order(Guid.Parse("125E6E34-9E83-4549-8D9A-4A654DFA32EA"), "Bob", 89.90m)
    ];

    public IReadOnlyCollection<Order> GetAll()
    {
        return Orders;
    }
}
