using TestArchitecturePoc.Domain.Entities;

namespace TestArchitecturePoc.Application.Abstractions;

public interface IOrderRepository
{
    IReadOnlyCollection<Order> GetAll();
}
