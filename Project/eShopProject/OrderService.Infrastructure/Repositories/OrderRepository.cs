using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Abstractions;
using OrderService.Domain.Models;
using OrderService.Infrastructure.EfCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Repositories
{
    public sealed class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _db;
        public OrderRepository(OrderDbContext db) => _db = db;

        public async Task<Order?> GetAsync(int id, CancellationToken ct = default) =>
            await _db.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Item)
                .FirstOrDefaultAsync(o => o.Id == id, ct);

        public async Task AddAsync(Order order, CancellationToken ct = default) =>
            await _db.Orders.AddAsync(order, ct);

        public Task SaveChangesAsync(CancellationToken ct = default) =>
            _db.SaveChangesAsync(ct);
    }
}
