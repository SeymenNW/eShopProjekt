using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Models
{
    public enum OrderStatus { Created = 0, Paid = 1, Shipped = 2, Canceled = 3 }

    public sealed class Order
    {
        private readonly List<OrderItem> _items = new();
        private Order() { } // EF

        public Order(string buyerId, Address shipTo)
        {
            Guard.Against.NullOrEmpty(buyerId);
            Guard.Against.Null(shipTo);

            BuyerId = buyerId;
            ShipToAddress = shipTo;
            OrderDate = DateTimeOffset.UtcNow;
            Status = OrderStatus.Created;
        }

        public int Id { get; private set; }
        public string BuyerId { get; private set; } = default!;
        public DateTimeOffset OrderDate { get; private set; }
        public Address ShipToAddress { get; private set; } = default!;
        public OrderStatus Status { get; private set; }

        public IReadOnlyList<OrderItem> Items => _items;

        public void AddItem(ItemSnapshot item, decimal unitPrice, int units)
        {
            Guard.Against.Null(item);
            Guard.Against.NegativeOrZero(unitPrice);
            Guard.Against.NegativeOrZero(units);

            var existing = _items.FirstOrDefault(i => i.Item.ItemId == item.ItemId && i.UnitPrice == unitPrice);
            if (existing is not null) { existing.IncreaseUnits(units); return; }

            // Pass the Order Id to the OrderItem constructor
            _items.Add(new OrderItem(item, unitPrice, units, this.Id));  // 'this.Id' is the Order's Id
        }


        public decimal Total() => _items.Sum(i => i.LineTotal);

        public void MarkPaid() => Status = OrderStatus.Paid;
        public void MarkShipped() => Status = OrderStatus.Shipped;
        public void Cancel() => Status = OrderStatus.Canceled;
    }
}
