using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Domain.Tests;

/// <summary>
/// Builder for creating test anemic models for the ReturnRequest context.
/// </summary>
internal static class TestAnemicModelBuilder
{
    public static IReturnRequestAnemicModel Build(
        string status = "Requested",
        string reason = "Product defective",
        string rmaNumber = "RMA-001",
        DateTime? requestedAt = null,
        decimal refundAmount = 0m,
        string? returnLabelUrl = null,
        string? returnLabelCarrier = null,
        int itemCount = 1,
        decimal unitPrice = 29.99m)
    {
        return new TestReturnRequestAnemicModel
        {
            Root = new TestReturnRequestRoot
            {
                Id = Guid.NewGuid(),
                OrderId = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                RmaNumber = rmaNumber,
                Reason = reason,
                Status = status,
                RequestedAt = requestedAt ?? DateTime.UtcNow
            },
            Items = Enumerable.Range(0, itemCount)
                .Select(i => new TestReturnItem
                {
                    VariantId = Guid.NewGuid(),
                    ProductName = $"Product {i + 1}",
                    Quantity = 1,
                    UnitPrice = unitPrice
                })
                .ToList<IReturnItem>(),
            ReturnLabel = returnLabelUrl is not null
                ? new TestReturnLabel
                {
                    LabelUrl = returnLabelUrl,
                    Carrier = returnLabelCarrier ?? "DHL"
                }
                : null,
            RefundAmount = refundAmount
        };
    }

    private sealed class TestReturnRequestAnemicModel : IReturnRequestAnemicModel
    {
        public IReturnRequestRoot Root { get; init; } = null!;
        public IReadOnlyList<IReturnItem> Items { get; init; } = new List<IReturnItem>();
        public IReturnLabel? ReturnLabel { get; init; }
        public decimal RefundAmount { get; init; }
    }

    private sealed class TestReturnRequestRoot : IReturnRequestRoot
    {
        public Guid Id { get; init; }
        public Guid OrderId { get; init; }
        public Guid CustomerId { get; init; }
        public string RmaNumber { get; init; } = string.Empty;
        public string Reason { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public DateTime RequestedAt { get; init; }
    }

    private sealed class TestReturnItem : IReturnItem
    {
        public Guid VariantId { get; init; }
        public string ProductName { get; init; } = string.Empty;
        public int Quantity { get; init; }
        public decimal UnitPrice { get; init; }
    }

    private sealed class TestReturnLabel : IReturnLabel
    {
        public string LabelUrl { get; init; } = string.Empty;
        public string Carrier { get; init; } = string.Empty;
    }
}
