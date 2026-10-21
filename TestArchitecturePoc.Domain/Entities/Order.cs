namespace TestArchitecturePoc.Domain.Entities;

public sealed class Order
{
    public Order(Guid id, string customerName, decimal amount)
    {
        Id = id;
        CustomerName = customerName;
        Amount = amount;
    }

    public Guid Id { get; }

    public string CustomerName { get; }

    public decimal Amount { get; }
}
