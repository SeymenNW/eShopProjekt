using OrderService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Abstractions
{
    public interface IOrderRepository
    {
        Task<Order?> GetAsync(int id, CancellationToken ct = default);
        Task AddAsync(Order order, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
