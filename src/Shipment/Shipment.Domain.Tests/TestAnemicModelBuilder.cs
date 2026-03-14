using Shipment.Domain.Abstraction;

namespace Shipment.Domain.Tests;

/// <summary>
/// Builder for creating test anemic models for the Shipment context.
/// </summary>
internal static class TestAnemicModelBuilder
{
    public static IShipmentAnemicModel Build(
        string status = "Pending",
        string trackingNumber = "TRK-001",
        string carrier = "DHL",
        string shippingAddress = "123 Main St",
        string? labelUrl = "https://labels.example.com/label.pdf",
        int itemCount = 1)
    {
        return new TestShipmentAnemicModel
        {
            Root = new TestShipmentRoot
            {
                Id = Guid.NewGuid(),
                OrderId = Guid.NewGuid(),
                TrackingNumber = trackingNumber,
                Carrier = carrier,
                ShippingAddress = shippingAddress,
                Status = status,
                CreatedAt = DateTime.UtcNow
            },
            Items = Enumerable.Range(0, itemCount)
                .Select(i => new TestShipmentItem
                {
                    VariantId = Guid.NewGuid(),
                    ProductName = $"Product {i + 1}",
                    Quantity = 1
                })
                .ToList<IShipmentItem>(),
            Label = labelUrl is not null
                ? new TestShippingLabel
                {
                    LabelUrl = labelUrl,
                    GeneratedAt = DateTime.UtcNow
                }
                : null
        };
    }

    private sealed class TestShipmentAnemicModel : IShipmentAnemicModel
    {
        public IShipmentRoot Root { get; init; } = null!;
        public IReadOnlyList<IShipmentItem> Items { get; init; } = new List<IShipmentItem>();
        public IShippingLabel? Label { get; init; }
    }

    private sealed class TestShipmentRoot : IShipmentRoot
    {
        public Guid Id { get; init; }
        public Guid OrderId { get; init; }
        public string TrackingNumber { get; init; } = string.Empty;
        public string Carrier { get; init; } = string.Empty;
        public string ShippingAddress { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }

    private sealed class TestShipmentItem : IShipmentItem
    {
        public Guid VariantId { get; init; }
        public string ProductName { get; init; } = string.Empty;
        public int Quantity { get; init; }
    }

    private sealed class TestShippingLabel : IShippingLabel
    {
        public string LabelUrl { get; init; } = string.Empty;
        public DateTime GeneratedAt { get; init; }
    }
}
